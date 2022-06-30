
Player Role Assignment System for VRChat v.1.0

Objective: 
Create a player role assignment mechanism for the Imaginary Theatre Company (ITC) VRChat project.

Description:
Players can get and release roles by entering portals. Owning specific roles produces consequences for the local world of role owners. Currently implemented with two roles: DoorOpener (interacts with doors) and FXController (controls sound and lights in the world).

Features:
-Entering the portal for the first time assigns the role specified by the portal; the second time releases it. 
-Portals have a maximum capacity which defines how many players can own that role.
-The system indicates on a custom UI the following: 1. current portal’s capacity, 2. the role linked to that portal 3. the action occurring when entering the portal (either get or release role).
-The portal's entrance changes color depending on the player’s current ownership of the role.
-Entering the portal triggers a sound fx as feedback.
-When the portal’s capacity reaches zero the portal turns unavailable and hidden to players not owning the portal's role.
-Owning a role leads to consequences in the world. For example, owning the FXController role makes players capable of seeing and interacting with light and sound floating controllers. Similarly, owning the DoorOpener role gives the ability to interact with doors.
-The system was built in modular pieces to easily scale and allow for future roles and consequences. 
-The system is synchronized for a multiplayer experience.

System Architecture:
PlayerRoleAssigner--> 1.RoleExecutor (FXRoleExecutor,DoorOpenerExecutor)-->InteractableController (LightSyncController, OneShotSoundSyncController).
					  2.OnScreenScrollingText                      			

Unity package dependecies:
The Unity project requires the following packages to load the scene correctly: UdonSharp, VRChatSDK, and ProBuilder.

Notes:
-There are two versions of the scripts. 1. Udon# version which contains synced scripts that work in VRChat, and are built using the language constraints at the moment. For instance,  Udon# scripts do not support inheriting from classes other than UdonSharpBehaviour which limits the use of a base class (RoleExecutor base class) from which different roles/executors are derived. Similarly, C# OR Unity events are not supported. The C# version scripts were only built to practice the application of OOP principles. They are not synced.
-VRChat does not support URP.
-For testing synchronization go to Menu: VRChat SDK - Show control panel - Builder - Build & test (num clients: 2+; Force none VR: true). Must have a VRChat account to access the Builder tab

Credits:
ASSETS
-Entering Portal SoundFX: Magical portal open by alanmcki; 4.0 International (CC BY 4.0); https://freesound.org/people/alanmcki/sounds/401324/;Attribution 
-Ambient Music: VRChat loading old version. https://www.youtube.com/watch?v=Ypg_4haO8ok
-Portal 3D Model: Low Poly Portal by N1x; CC Attribution; https://sketchfab.com/3d-models/low-poly-portal-38e1dec9d32d4a768f8e810848b7b59b

PROGRAMMING by Daniel Ruiz. ruizleyvadaniel@gmail.com


