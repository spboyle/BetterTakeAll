# Better Take All
Ever recover a Tombstone and wonder why you have two partial stacks of wood and a key item was left in the tombstone?

This mod addresses that, by changing TakeAll behavior for both tombstones and other containers to
- prefer stacking if possible
- then prefer position of item in source container
- then try to put it anywhere

This should address most failures to retrieve all items from tombstone, and also fixes the rare edge case of failure to stack when selecting "Take All" on a chest or other container.

## Details
When you interact with a tombstone,
1. The system checks if it thinks the tomstone inventory will fit into the player's inventory
2. If yes, it performs `TakeAll`, which calls `MoveAll`, which moves items from source container (tomsbtone) to destination container (player's inventory)
3. If no, it opens up the tombstone like a regular container for the player to manually move stuff

This mod changes `Inventory.MoveAll` behavior which affects all containers including tombstones. Normally, MoveAll prefers to put an item in its exact slot from the source container, and failing that, it looks for a stackable item in the destination container. This is why with tombstones, if you pick up some stackable junk along the way, it often fails to stack things properly.

## What it doesn't do
This mod doesn't address the `Tombstone.EasyFitInventory` method, which checks if it should even automatically attempt to `TakeAll` in the first place. If your inventory is especially convoluted when you die, e.g. with several partial stacks of the same item, the tombstone will usually not attempt `TakeAll`.

## Other mods that improve tombstone behavior
These two mods and my own all compete over `Inventory.MoveAll`, so they're not compatible. If installed together, these mods take precedence over my own.
* [Simple Smarter Corpse Run And Tombstone](https://thunderstore.io/c/valheim/p/Goldenrevolver/Simple_Smarter_Corpse_Run_And_Tombstone/) - intelligently predicts your carrying capacity instead of giving you a flat 150 extra, to avoid being overburdened after Corpse Run buff ends
* [TombstoneDriveBy](https://thunderstore.io/c/valheim/p/sephalon/TombstoneDriveBy/) - Never open up tombstone like a regular container, just auto-take stuff in a hardcoded priority order
