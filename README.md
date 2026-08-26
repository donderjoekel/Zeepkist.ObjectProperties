# Zeepkist.ObjectProperties

Object Properties is a mod for the Zeepkist level editor. The mod adds extra fields to the object inspector.

You can type exact values for properties such as booster force and fan range.

## Installation

Install Object Properties with [Modkist](https://github.com/Thundernerd/ModkistMKII).
Modkist installs BepInEx and ZeepSDK.

## How to use

After installation, the inspector in the level editor changes.

Do these steps:

1. Open the level editor in Zeepkist.
2. Select a block.

The inspector shows regular properties and advanced properties together.

Object Properties does not link the X, Y, and Z scale values. You can set a different scale for X, Y, and Z.

### Extra properties

When you select a supported block, Object Properties adds extra fields to the inspector. The field labels use the game localization.

The extra fields for each block type are:

- **Boosters**: Acceleration and value
- **Slowsters**: Acceleration and value
- **Fans**: Range and force
- **Feather forcefields**: Height, damping, and velocity
- **Cycle blocks**: Cycle offset (not shown on fans)
- **Logic gates**: Height 1 and distance 1 for all gates, and height 2 and distance 2 for two-input gates
- **Monorail**: Speed
- **Custom lights**: Color, saturation, and brightness.
