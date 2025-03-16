# Dragon-Simu
 hola
scripts: 
1. PlaceOnPlaneNewInputSystem.cs:
->uses AR Raycast Manager to detect planes and place the dragon
->prevents re-positioning while using the joystick
setup-:
->attach script to an empty GameObject in the scene.
->ssign the dragon Prefab in the Inspector.
->assign the fly Button and fire Button so that they control the spawned dragon instance.
2. DragonController.cs:
-> moves the dragon based on joystick input.
-> toggles between walking and flying states
-> plays the correct fire animation depending on the state (walking aur flying)
setup:-
-> attach this script to the dragon Prefab.
-> dragon should has a Rigidbody component (for physics-based movement).
->isWalking, isFlying, FlameAttack, FlyFlameAttack parameters on animator.

animator:-
Idle → Walk → Idle: controlled via isWalking bool
Idle → Fly → Idle: Controlled via isFlying bool

ui button assignment:-
Fly Button → calls ToggleFlightMode()
Fire Button → calls FireAttack()
