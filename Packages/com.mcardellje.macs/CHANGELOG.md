## MACS [1.7.2] - 2025-09-17

### Fixed
- Default Write Defaults Mode is functional again

## MACS [1.7.1] - 2025-09-09

### Fixed
- Editor will no longer stutter when pressing a modifier key (Ctrl, Shift, Alt) in a large scene

## MACS [1.7.0] - 2025-06-26

### Added
- F2 to Rename to States, Layers, BlendTrees and Parameters (setting under Minor Fixes)

## MACS [1.6.17] - 2025-06-09

### Fixed
- Prevented some harmless error messages when looking at blend trees

## MACS [1.6.16] - 2025-05-31

### Fixed
- Blend Tree Create AAP Clip menu no longer prevents you from selecting the AAP value input field
- Blend Tree initial parameters are no longer random (unity bug)

## MACS [1.6.15] - 2025-05-25

### Fixed
- Add Blend Tree and Paste Blend Tree now respect default direct blend parameter setting

## MACS [1.6.14] - 2025-04-25

### Added
- Automatic transition condition conversion when changing parameter types

### Fixed
- Parameter context menu will no longer fail to open when there is no VRCAvatarDescriptor
- Parameter context menu will no longer fail to open when there is a parameter list but no menu in the VRCAvatarDescriptor

## MACS [1.6.13] - 2025-04-23

### Fixed
- "Icon not found" warnings should no longer appear unless the icon is actually missing

### Changed
- Made speed parameter icon tooltip more verbose

## MACS [1.6.12] - 2025-04-17

### Fixed
- Parameter context menu will no longer fail to open when there is no parameter list in the VRCAvatarDescriptor
- Creating and pasting transitions to sub-StateMachines will no longer fail under certain conditions

## MACS [1.6.11] - 2025-03-07

### Added
- Proper warning message for invalid items in VRC menus

### Changed
- Major Changes to external BlendTree creation:
  1. Regardless of settings, when a BlendTree is added inside a BlendTree that is stored externally to the AnimatorController, that new BlendTree is always created inside the same asset as its parent (no more external blendtrees linking back to internal blendtrees)
  2. with Create External Blend Trees enabled, if the first rule did not apply, then a new BlendTree will be created externally based on the Blend Tree Output Directory setting
  3. with Create External Blend Trees disabled, if the first rule did not apply, then a new BlendTree will be created inside the Animator Controller

## MACS [1.6.10] - 2025-02-05

### Added
- Plus (\+) button to create a new animation clip to the Fuzzy Match Clip Selection popup

### Changed
- Fuzzy Match Clip Selection now has arrow key navigation, Enter to select, and Escape to close (Ctrl+Enter to begin recording)

### Fixed
- Create External Blend Trees producing errors when adding blendtrees

## MACS [1.6.9] - 2024-07-12

### Changed
- Parameter "Make Synced" menu is now named "VRC Parameter" and can now remove and change type / sync status of VRC Parameters

### Fixed
- Make BlendTree Internal should now work on BlendTrees that are contained inside of an animator state and are not the child of another BlendTree
- Triggers are now supported when changing parameter types and for transition condition labels

## MACS [1.6.8] - 2024-05-27

### Added
- Setting to make transition labels horizontal for improved readability (might make labels more likely to overlap)
- Setting to make transition labels have an outline for improved readability

### Changed
- Write Defaults warning will now show the WD indicator even if it is disabled
- Non-synced parameter icon is now gray instead of green to make it different from the synced icon

### Fixed
- Parameter type mismatch display now takes into account avatar dynamics

### Removed
- "Refusing to reparent blendtree to self" warning due to it spamming the logs and not being useful

## MACS [1.6.7] - 2024-05-20

### Added
- Setting to change the default parameter name for direct blend tree weights

### Fixed
- Error when using "Find Usages in Animator" on a motion when no Animator window was open or no Animator Controller was selected
- Issues with all "Find" windows not working correctly with substatemachines

## MACS [1.6.6] - 2024-03-24

### Added
- Setting to disable the "VRChat SDK spawning broken animator windows" fix
- Add a setting to set the default Write Defaults mode for new states

### Fixed
- m_ActiveElement not found exception when adding a new parameter above / below

## MACS [1.6.5] - 2024-03-03

### Added
- IsOnFriendsList Parameter

### Fixed
- Parameter not found exception when non-SRC MACS is loaded


## MACS [1.6.4] - 2024-02-25

### Fixed
- NullReferenceException when there is no animator window open and a VRC Avatar Parameter Driver is visible in the inspector

## MACS [1.6.3] - 2024-02-25

### Added
- Transition labels for "Always" transitions will now show "End of Motion" if exit time is 1 and "After X%" if exit time is not 1 instead of "Always"
- Fix for VRChat SDK spawning broken animator windows

## MACS [1.6.2] - 2024-01-28

### Added
- View All Animations context menu item on GameObjects with Animators and/or Avatar Descriptors
- View Animations context menu item on layers
- Icons in the Motion Viewer (Find What Animates This) window
- Motion Viewer window is now able to show blend trees
- Find Usages In Animator for all motion assets

### Fixed
- What Animates window now has properly aligned headers (finally!)
- Find What Animates This no longer looks in controllers attached to the Avatar Descriptor whose layers are set to use the default controller instead
- Find What Animates This now looks in Avatar Descriptor "special" layers too

## MACS [1.6.1] - 2024-01-03

First version of 2024!

### Fixed
- NullReferenceException when right clicking a parameter when the VRC Expressions Menu contains a submenu item with no submenu
- T and V keybinds not working in 2022
- Right clicking a parameter throwing an error when there is no VRC Avatar Descriptor connected to the animator

### Changed
- Disabled "Custom Parameter Descriptions" and "Improve List Performance" on 2022 because they are broken and list performance is already improved in 2022

## MACS [1.6.0] - 2023-12-09

### Added
- You can now drop objects and components on to BlendTrees to create toggle animations for them
- New context menu items on parameters, "Copy Name" and "View in Expressions Menu"

### Fixed
- Variety of fixes for getting MACS to work in Unity 2022
- Stopped including 0Harmony.dll (LibHarmony) in new versions of SRC and VRC MACS because it is now included in the VRCSDK
- Made "Find What Animates This" window work with Object Reference curves (material swaps)
- Made Parameter "Make Synced" copy the default value for the new expressions parameter from the animator parameter

## MACS [1.5.1] - 2023-10-09

### Fixed
- "ArgumentOutOfRangeException" when changing transition condition parameter to a parameter of a different type when Don't Reset Condition Mode is enabled

## MACS [1.5.0] - 2023-09-27

### Added
- BlendTree "Replace Parent" context menu, to replace a blend tree's parent with the selected child, effectively shuffling the blend tree up one level, deleting the old parent
- Keybinds now listed in README.md
- Pressing Ctrl+Enter on the fuzzy match clip selection window will now select the first clip found in the list
- Pressing the plus button on the Animator Parameters List now gives the option to directly add one of the VRChat built-in parameters
- Right clicking in the animation fuzzy match search window immediately starts recording the selected animation

### Changed
- Lock inspector keybind is now CTRL+L and duplicate inspector keybind is now I / CTRL+SHIFT+I / SHIFT+I to avoid conflicts with the default unity keybinds
- The animation fuzzy match search window now remembers last search filter and only requires single click to select an animation

### Fixed
- Made BlendTrees no longer have their thresholds reset when copied
- Error when trying to change the parameter of a condition when the previous parameter did not exist

## MACS [1.4.0] - 2023-08-04

### Added
- Option to shorten parameter type names down to their first letter
- Option to change how the clip selection drop down is displayed, either using submenus or fuzzy search

## MACS [1.3.0] - 2023-08-03

### Added
- Icons for the parameter ordering dropdown (If your OS supports it, it probably doesn't)

### Fixed
- "Icon not found" warning due to misnamed icon
- Improved performance of parameter usage icon

## MACS [1.2.0] - 2023-07-29

### Added
- Parameter usage icon, indicating whether a paramater is unused, written by AAP, or driven by parameter driver
- Blend Tree context menu items to make an internal copy of a blend tree or make the blend tree external
- Clicking on the VRChat Builtin parameter icon will now take you to the relevant documentation
- "Open Documentation" context menu item on VRChat state behaviours
- The "Help" button now works on all VRChat components and state behaviours, instead of trying to take you to the regular unity documentation it now takes you to the relevant VRChat documentation
- Clicking on the parameter type will now open a drop down to select a new type

### Changed
- Reordered the blend tree context menu items to follow a more logical order
- Instant transition arrow colour is now cyan, green conflicted with the solo transition arrow colour

### Fixed
- Broken transitions (with invalid conditions) now always show their status text, to make them more obvious
- Missing package.json causing the package to not be imported correctly

## MACS [1.1.1] - 2023-07-29

### Fixed
- Leftover debug code that logged to console when duplicating inspector window

## MACS [1.1.0] - 2023-07-29

### Added
- Changelog and make versioning more obvious on download segment
- Semantic versioning (for now)
- Clicking on clipboard status text now clears clipboard
- New scaling parameter support for VRChat Builtin parameter icon
- Keybind for toggling inspector lock (L)
- Keybind for creating a duplicate, locked, inspector window (D -> horizontal split, Ctrl+D -> vertical split, Ctrl+Shift+D -> new tab)
- Keybind for closing an inspector window (Ctrl+W)