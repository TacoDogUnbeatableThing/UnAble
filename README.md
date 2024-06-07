# Unity Mod Template

A BepInEx template to quickly make a unity mod.

The game directory is specifiable in `UnAbleMod.sln`

Building will auto copy the mod DLL into the BepInEx plugins folder

## Setup

The mod should run out of the box! However to quickly rename it run the `Rename.ps1` script in powershell.


## Making things easier: Build + Run

Add a run/debug configuration

Before launch: Build solution

Script text:
`& "<GAME EXE FULL PATH>"`
