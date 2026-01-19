using System;
using UnityEngine;

namespace TGL.ServiceLocator
{
	/// <summary>
	/// Automates the initial setup, configuration, and loading of components
	/// </summary>
	[DisallowMultipleComponent, RequireComponent(typeof(SLocator))]
	public abstract class Bootstrapper : MonoBehaviour
	{
		private SLocator attachedLocator;
		internal SLocator AttachedServiceLocator
		{
			get
			{
				if (attachedLocator != null)
				{
					return attachedLocator;
				}
				else if(GetComponent<SLocator>() != null)
				{
					attachedLocator = GetComponent<SLocator>();
				}
				
				return attachedLocator;
			}
		}

		private bool hasBeenBootstrapped;

		private void Awake()
		{
			BootstrapOnDemand();
		}

		/// <summary>
		/// Attaches and configures a <see cref="SLocator"/> by calling it's configure methods<br/>
		/// <see cref="SLocator.ConfigureAsGlobal"/>, <see cref="SLocator.ConfigureForScene"/> and <see cref="SLocator.ConfigureForGameObject"/> methods
		/// </summary>
		public void BootstrapOnDemand()
		{
			if(hasBeenBootstrapped) return;
			hasBeenBootstrapped = true;
			Bootstrap();
		}
		
		protected abstract void Bootstrap();
	}
}