# Create an RPG Game in Unity

## Links
Course Link:
- https://juegostudios.udemy.com/course/create-an-rpg-game-in-unity/learn/lecture/31236604#overview
- https://www.udemy.com/course/create-an-rpg-game-in-unity/?couponCode=CP250105G1
  Instructor:
- https://www.udemy.com/user/pete-jepson/

## Information
##### Game
Information about the game:
- Game is a 3d RPG    
- We are using Navmesh for AI pathfinding.
  - We are using point to move system for player movement.
- 

##### Assets
Assets used in the project:
- Unity Packages:
    - Download the 'Medieval Cartoon Warriors' unity package and update for Unity 6000
    - Downloaded the Terrain for use from : [Udemy course](https://att-c.udemycdn.com/2022-03-14_13-41-21-2b456ce87b63b6bf30e5b8f07a1382b1/original.zip?response-content-disposition=attachment%3B+filename%3DTerrainLevel1.zip&Expires=1768323356&Signature=cUE34VJ3fUGLhcKscD-HRGa4yP-LQpJ6~Psqm28aPic7pcKGeL7~4WsriHNTsS9TboG0mSOAc9fRjMUNuSxrt4H-GIwasyRwUDFxI0mt8UJJ4g0jY2wn3GpDu2Kkm-n2HoPcgdLh2Ug71G2GXHXxSC~yCnw-A7BgtU~zBnOwce8P54W3cuzNaVmIoJjYIw~ZMAx1NAvYjGFmSE9llcGGwaPrkizZFfAqJUV~596FQXp2zSS09m1Rht5GOaZwRxJJ0Z2uTen4MUKgU7-rv85FyYpWeYwnJFAQ~WJzFC70iaj9ruG-CSKKc4StftBX3Uip8FKlfFmmfcCa6bKQ03Y2SA__&Key-Pair-Id=K3MG148K9RIRF4)
- Asset store
    - [Medieval house modular v2.0 - lite - URP](https://assetstore.unity.com/packages/3d/environments/fantasy/medieval-house-modular-v2-0-lite-urp-189718) - [Mikel Olaizola](https://assetstore.unity.com/publishers/52505)
    - [Alchemy Lab Props](https://assetstore.unity.com/packages/3d/props/furniture/alchemy-lab-props-41758) - [M. na Station](https://assetstore.unity.com/publishers/12379)
    - [Lowpoly Dinner Table](https://assetstore.unity.com/packages/3d/environments/fantasy/lowpoly-dinner-table-55180) - [Evgenia](https://assetstore.unity.com/publishers/9175)
    - we are downloading all assets from the Udemy course asset links.

##### Packages
Packages:
- `com.unity.postprocessing` - 3.5.1
- Pro Builder - removed when we downloaded complete asset from udemy course.
- `com.unity.cinemachine` - 3.1.5
- 

#### Changes based on Platform
What changes are needed based on platform:
- We are *using Linear color space*, so if the target device is low end/old phones, we might have to change color and textures while changing color space to 'Gamma'.
- For *Mobile (Android/iOS)* **Fast sRGB/Linear Conversion:** In your *URP Asset* (under Post-processing), there is an option for *Fast sRGB/Linear Conversion*. "Enable this". It uses an approximation that is much faster on mobile GPUs with almost zero visual difference.


### Systems
#### Constants
- All constant values are stored in `GameConstants.cs` file.
- We made multiple partial classes for `GameConstants.cs` file to organize constants based on their usage.
  - see `GameConstants.PlayerAnimConstants` class for understanding.

#### Player Movement
Using the namespace `TGL.RPG.Navigation.PTM`(Point To Move) for player movement.
- Player movement is done using Navmesh and raycasting.
- Player clicks on the ground to move to that position.
- `PlayerAnimationController.cs` is used to control the player animations based on movement.
- `AutoBraking` is turned off, so character does not slow down when approaching the target point.

#### Camera System
Using the Cinemachine package for camera system.
- `CinemachinePlayerLook` is used for controlling the cinemachine camera.

