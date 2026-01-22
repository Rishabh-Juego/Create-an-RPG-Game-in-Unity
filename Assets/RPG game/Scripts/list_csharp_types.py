import os
import re
import sys
import argparse
from collections import defaultdict

"""
Can use this script to audit C# files in a Unity project for the following:
1. Identify structures (classes, interfaces, structs, enums) without namespaces.
2. Identify files containing multiple structures.
3. Generate a grouped and sorted list of all structures by their namespaces.
Usage:
1. Current Directory: Open your terminal in the Unity folder and run: 
python3 list_csharp_types.py

2. Specific Directory: 
python3 list_csharp_types.py "/Users/rishabh/Documents/UnityProjects/..."
"""

def strip_comments_and_strings(code):
    """
    Removes comments and string literals to prevent false positives 
    during parsing.
    """
    # Remove multi-line comments /* ... */
    code = re.sub(r'/\*.*?\*/', '', code, flags=re.DOTALL)
    # Remove single-line comments // ...
    code = re.sub(r'//.*', '', code)
    # Remove string literals "..." and @"..." to avoid finding keywords in strings
    code = re.sub(r'@?"[^"\\\r\n]*(?:\\.[^"\\\r\n]*)*"', '', code)
    return code

def parse_cs_file(file_path):
    with open(file_path, 'r', encoding='utf-8') as f:
        raw_content = f.read()

    # Clean the code to look only at actual declarations
    clean_code = strip_comments_and_strings(raw_content)

    # 1. Extract Namespace
    # Supports: 'namespace A.B;' and 'namespace A.B {'
    ns_match = re.search(r'\bnamespace\s+([\w\.]+)', clean_code)
    namespace = ns_match.group(1) if ns_match else None

    # 2. Extract Structures
    # Matches: class, interface, struct, enum
    struct_matches = re.findall(r'\b(class|interface|struct|enum)\s+([\w<>]+)', clean_code)
    
    return namespace, struct_matches

def run_audit(target_dir):
    ns_map = defaultdict(list)
    missing_ns = []
    multi_struct_files = []

    for root, _, files in os.walk(target_dir):
        for file in files:
            if file.endswith(".cs"):
                full_path = os.path.join(root, file)
                namespace, structures = parse_cs_file(full_path)

                if not structures:
                    continue

                # Requirement 1: Log missing namespaces
                if namespace is None:
                    for _, name in structures:
                        missing_ns.append(f"{name} (in {file})")
                    namespace = "[MISSING NAMESPACE]"

                # Requirement 2: Log multiple structures in one file
                if len(structures) > 1:
                    struct_names = [name for kind, name in structures]
                    multi_struct_files.append((file, struct_names))

                # Requirement 3 & 4: Store for grouped/sorted list
                for kind, name in structures:
                    ns_map[namespace].append((name, kind))

    return ns_map, missing_ns, multi_struct_files

def display_report(ns_map, missing_ns, multi_files):
    print("="*50)
    print("C# PROJECT STRUCTURE AUDIT")
    print("="*50 + "\n")

    if missing_ns:
        print("!!! STRUCTURES WITHOUT NAMESPACES !!!")
        for item in sorted(missing_ns):
            print(f"  - {item}")
        print("\n")

    if multi_files:
        print("!!! FILES CONTAINING MULTIPLE STRUCTURES !!!")
        for file, names in multi_files:
            print(f"  - {file}: {', '.join(names)}")
        print("\n")

    print("STRUCTURES BY NAMESPACE (Alphabetical)")
    # Sort namespaces alphabetically
    for ns in sorted(ns_map.keys()):
        print(f"\n[{ns}]")
        # Sort structures within the namespace alphabetically
        sorted_structs = sorted(ns_map[ns], key=lambda x: x[0])
        for name, kind in sorted_structs:
            print(f"  {kind: <10} : {name}")

if __name__ == "__main__":
    parser = argparse.ArgumentParser(description="Audit C# files for namespaces and structures.")
    parser.add_argument("directory", nargs="?", default=os.getcwd(), 
                        help="The directory to scan (default: current directory)")
    
    args = parser.parse_args()

    if not os.path.isdir(args.directory):
        print(f"Error: {args.directory} is not a valid directory.")
        sys.exit(1)

    ns_map, missing_ns, multi_files = run_audit(args.directory)
    display_report(ns_map, missing_ns, multi_files)