# Bottling System Implementation Guide

## Overview
This document explains how the bottling date and bottler tracking system works in the Dated Liquor mod.

## What Was Implemented

### 1. **Bottling Information Storage**
When a bottle is filled (transitions from empty to having liquid), the system now stores:
- **Bottler Name**: The player who filled the bottle (`bottledBy` attribute)
- **Bottling Date**: The in-game calendar date when bottled (`bottledOnTotalDays` attribute)
- **UTC Timestamp**: Backup timestamp for external tools (`filledUtcTicks` attribute)

### 2. **Key Methods Added**

#### `StampFilled(ItemStack containerStack, IPlayer byPlayer = null)`
- Records bottling information to the container's attributes
- Stores the player's name and the current game calendar date
- Called automatically when a bottle is filled

#### `ClearBottlingInfo(ItemStack containerStack)`
- Removes all bottling information from a container
- Called when:
  - Bottle is emptied
  - Bottle is refilled (new liquid added to existing content)

#### `CheckAndStampIfFilled(ItemStack containerStack, IPlayer byPlayer = null)`
- Checks if a bottle transitioned from empty to filled
- Automatically calls `StampFilled` if conditions are met

### 3. **Detection of Bottle Filling**

The system detects when a bottle is filled by:
1. **OnHeldInteractStop Override**: 
   - Captures the state before and after player interactions
   - Detects when content changes from empty to filled
   - Automatically stamps the bottle with bottling info

2. **Emptying Detection**:
   - When a bottle is emptied (in `tryEatStop`), bottling info is cleared
   - This ensures old dates don't persist on empty bottles

### 4. **Display of Bottling Information**

The `GetHeldItemInfo` method now displays:
- **Bottler Name**: "Bottled by: [PlayerName]"
- **Age Information**: 
  - If aged 1+ years: "Aged X years, Y months"
  - If aged 1+ months: "Aged X months, Y days"
  - If less than a month: "Aged X days"

The age is calculated using the game's calendar system, so it reflects in-game time, not real-world time.

## Where Changes Were Made

### File: `datedliquor/src/BlockClass/BlockLiquidContainerCorkable.cs`

1. **Lines ~678-730**: Updated `StampFilled` method and added `ClearBottlingInfo` and `CheckAndStampIfFilled` helpers

2. **Lines ~357-375**: Enhanced `OnHeldInteractStop` to detect filling and stamp bottles

3. **Lines ~512-516**: Added clearing of bottling info when bottle is emptied

4. **Lines ~576-620**: Enhanced `GetHeldItemInfo` to display bottling information

### File: `datedliquor/assets/datedliquor/lang/en.json`

Added language entries:
- `datedliquor:bottled-by`: "Bottled by: {0}"
- `datedliquor:bottled-info-years`: "Aged {0} years, {1} months"
- `datedliquor:bottled-info-months`: "Aged {0} months, {1} days"
- `datedliquor:bottled-info-days`: "Aged {0} days"

## How It Works

### Filling Flow:
1. Player interacts with bottle (right-click to fill from water source, etc.)
2. Base class handles the liquid transfer
3. `OnHeldInteractStop` checks if bottle went from empty → filled
4. If yes, `StampFilled` is called with player info
5. Bottling date and bottler name are stored in item attributes

### Display Flow:
1. Player inspects bottle (looks at tooltip)
2. `GetHeldItemInfo` checks for bottling attributes
3. If found, calculates age from bottling date to current date
4. Displays bottler name and age information

### Clearing Flow:
1. When bottle is emptied, `ClearBottlingInfo` is called
2. All bottling attributes are removed
3. Next time bottle is filled, new bottling info will be recorded

## Important Notes

### Edge Cases to Consider:
1. **Automated Filling**: If other mods or systems fill bottles programmatically, they may not trigger `OnHeldInteractStop`. You may need to add additional hooks if this becomes an issue.

2. **Partial Filling**: The system only stamps when a bottle goes from completely empty to having content. If you want to track when additional liquid is added to a partially filled bottle, you'd need additional logic.

3. **Corking/Uncorking**: Currently, corking/uncorking doesn't affect the bottling date. The date persists through corking operations, which matches your TODO requirements.

### Future Enhancements:
- Add configuration option for date display format
- Track multiple fill events (refill history)
- Add bottling date to corked bottle display even when uncorked
- Integration with recipe system for automatic bottling date assignment

## Testing

To test the system:
1. Get an empty bottle
2. Fill it from a water source or other liquid container
3. Inspect the bottle - you should see "Bottled by: [YourName]" and age information
4. Empty the bottle - bottling info should disappear
5. Fill it again - new bottling info should appear

## Troubleshooting

If bottling info doesn't appear:
- Check that `OnHeldInteractStop` is being called (check logs for "OnHeldInteractStop" event)
- Verify the bottle actually has content (`GetContent` returns non-null)
- Check that attributes are being set (use debug mode to inspect itemstack attributes)

