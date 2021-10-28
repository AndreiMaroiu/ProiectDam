=== NERA SPRITE PACK ===
A metal slug inspired sprite pack. 
Includes sprites of a soldier girl named Nera.

=== USAGE ===
Sprites are designed for a metal slug styled game, where the leg and torso states are independent.
As a result, there are usually two sets of sprites for each animation, which can be used in different combinations.
Transitional sprites are also included, marked the "start" or "end" prefix.

While most sprites can simply be layered, there are some exceptions:
 1. When using 'shoot' torso sprites with 'run' leg sprites (including start and end), the torso sprite shoot be offset up by 1 pixel
 2. Shoot, melee, and jump sprites have different dimensions than the rest of the sprites, so some extra alignment will be necessary

=== SPRITE LIST ===
 Folder location: l = leg, t = torso

 - idle (l/t): Basic idle. Torso can be swap for shoot.
 - idle_alt (l): Alternate idle. Doesn't really fit with the rest of the pack.
 - shoot (t): Shooting.

 - run (l/t): Run. For all run sprites, torso can be swap for shoot, but should be offset up one pixel.
 - run_start (l/t): Transition from idle to run.
 - run_stop (l/t): Transition from run to idle.

 - jump (l): Jump legs. Works with idle and shoot torso

 - crouch_idle (l): Crouching idle.
 - crouch_move (l): Moving while crouched.
 - crouch move start-end (l): Single frame transition between idle and move
 - crouch_shoot (l): Shooting while crouched. Should be stationary.
 - crouch_start (l): Transition from idle/run to crouch.
 - crouch_end (l): Transition from crouch to idle

 - look_up (t): Idle, but looking up
 - shoot_up (t): Shooting upwards. Works with any sprite that the regular 'shoot' works with.
 - shoot_up_start (t): Look-up to shoot-up
 - shoot_up_end (t): Shoot-up to look-up
                                                                                                         
 - melee_knife (l): Full body, two-hit melee attack.                                                                                        


=== NOTES ===
 - Sprites that are not split into legs and torso (crouch and melee) will be located in the leg folder
 - Several vital sprites are missing, including shooting in a downward direction and hurt/death sprites.
   These may or may not be added in a future update, but ratings and donations would definitely help convince me to do do ;)


=== CREDITS ===
Credits (just mention 'poohcom1') are required, unless you donate. 