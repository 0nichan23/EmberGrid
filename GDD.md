# EmberGrid - Game Design Document

---

## 1. Game Overview

**Title:** EmberGrid
**Genre:** 2D Top-Down Grid-Based Turn-Based Tactical CRPG
**Engine:** Unity (C#)
**Target Audience:** Hardcore RPG players who enjoy deep character building, tactical combat, and long-term progression systems.

### Inspirations
- **Fire Emblem** - Tactical grid combat, Canto movement, mission-based structure
- **Deep Rock Galactic** - Loadout system, weapon skill trees, overclocks, class identity
- **Dungeons & Dragons / Pathfinder** - Fantasy archetypes, ability tiers, resource management
- **Dragon Age 2 / Inquisition** - Skill tree structure (base tree + specialization)

### Core Fantasy
The player commands a squad of fantasy heroes, each deeply customizable through weapons, gear, and skill trees. Every mission is a tactical puzzle where team composition, loadout choices, and in-combat decisions all matter. Mastery comes from understanding each class's strengths, building synergistic teams, and adapting to varied mission types and environments.

---

## 2. Core Gameplay Loop

1. **Draft Phase** - Select 3 heroes from a roster of 6 for the upcoming mission.
2. **Loadout Check** - Each drafted hero must have a valid loadout equipped (weapons + utility item).
3. **Mission** - Deploy on a procedurally generated map. Complete the objective through grid-based tactical combat.
4. **Rewards** - Earn resources, currency, and XP for participating heroes.
5. **Advancement** - Spend earned resources on skill trees, unlock new gear tree lines, progress toward specializations and overclocks.
6. **Repeat** - Draft a new team (or the same one) and take on the next mission.

All hero progression is **persistent** across missions. The player chooses how to distribute investment - focusing on a core team of 3 for faster progress or spreading resources across all 6 for roster flexibility.

---

## 3. Combat System

### Grid & Tiles
- Combat takes place on a 2D grid. Each tile can be occupied by one unit.
- **No Unity physics** - all distance, targeting, and hitbox calculations are grid-based.
- Tiles have properties based on terrain (walkable, blocked, hazardous, etc.).

### Movement
- Each unit has a **Movement Speed** stat representing the number of tiles they can move per turn.
- Movement is **free and splittable** (Canto system): a hero can move, perform an action, then continue moving with remaining movement points.
- Movement does not consume Action Points.

### Action Points (AP)
- All units default to **1 AP** per turn.
- AP is spent to use abilities (at-will, power, ultimate).
- Certain effects (e.g., Haste) can temporarily grant additional AP.
- The Martial class has access to a periodic bonus AP as part of its kit.

### Turn Structure
- **Player Phase** - The player moves and acts with all 3 heroes in any order.
- **Enemy Phase** - All enemies take their turns.
- Turn order within a phase is player-controlled (for heroes) or AI-controlled (for enemies).

### Enemy Intent
- During the Player Phase, enemy intent is **visible** (e.g., which hero an enemy plans to target, what action they intend to take).
- Enemy intent **recalculates** after each hero finishes their turn, reflecting the changed board state.
- This allows the player to make informed tactical decisions and manipulate enemy behavior through positioning.

---

## 4. Heroes & Classes

The game features **6 playable classes**. Each represents a distinct fantasy archetype with a unique playstyle, weapon set, and class identity.

### 4.1 The Martial
**Identity:** Master of martial combat. The most straightforward class in the game.
**Playstyle:** Heavy hitting, tanky, forgiving. Built for players who want reliable power without complex execution.
**Key Features:**
- Self-cleanse for status effects (utility)
- Periodic bonus Action Point (every few rounds)
- High durability, strong melee presence
**Weapons:** Martial/physical weapons (swords, axes, hammers, etc.)

### 4.2 The Arcanist
**Identity:** Arcane spellcaster and master of classic evocation magic.
**Playstyle:** Elemental damage dealer with access to AoE and single-target spells. Versatile offensive toolkit.
**Key Features:**
- Mana-based resource system
- Access to multiple damage types (fire, ice, lightning, etc.)
- Strong AoE potential
**Weapons:** Wands, spellbooks, orbs

### 4.3 The Primalist
**Identity:** Warrior of nature who can specialize in almost anything.
**Playstyle:** The most versatile class. Can be built as healer, damage dealer, summoner, or hybrid depending on spec and loadout.
**Key Features:**
- Healing capability
- Nature-based summons (beasts, elementals)
- Broad specialization options
**Weapons:** Wooden staves, totems

### 4.4 The Diviner
**Identity:** Devout follower of a higher entity. A divine spellcaster.
**Playstyle:** Support/healer with divine offensive options. The most natural healer in the roster.
**Key Features:**
- Primary healing class
- Divine magic (buffs, restoration, smites)
- Team sustain focus
**Weapons:** Divine implements (holy symbols, prayer books, censers, etc.)

### 4.5 The Dexterous
**Identity:** Swift fighter who uses skill and precision.
**Playstyle:** High-risk, high-reward "giant slayer." Unforgiving to play but devastating in skilled hands. The thinking player's class.
**Key Features:**
- High single-target burst damage
- Multiple execution paths (different ways to achieve kills depending on build)
- Trapper specialization (summons traps for crowd control)
- Requires smart positioning and planning
**Weapons:** Precision weapons (daggers, crossbows, throwing weapons, etc.)

### 4.6 The Occultist
**Identity:** Dark user of forbidden magic.
**Playstyle:** Forbidden/dark magic with powerful but potentially costly abilities.
**Key Features:**
- Dark magic abilities
- Undead summoning
- Thematically distinct from the Arcanist (dark/forbidden vs. classical/academic)
**Weapons:** Skull implements, ritualistic swords, cursed tomes

---

## 5. Loadout System

### Structure
Each hero's loadout consists of:

| Slot | Selection | Source |
|------|-----------|--------|
| Primary Weapon | Choose 1 of 2 (class-specific) | Grants 3 abilities |
| Secondary Weapon | Choose 1 of 2 (class-specific) | Grants 3 abilities |
| Utility Item | Fixed per class (no selection) | Grants 2 abilities |

A hero must have a valid loadout to be drafted for a mission.

### Weapons
Each class has access to **2 primary weapons** and **2 secondary weapons**, all unique to that class. Weapons are the primary source of active abilities and define the hero's combat options.

The weapons between classes are thematically and mechanically distinct. An Arcanist's wand plays nothing like an Occultist's skull implement, even if both are "caster" archetypes.

### Utility Items
Each class has **1 utility item** (fixed, not a choice). Customization comes from the utility item's skill tree, which branches into different ability options.

Every class has access to a **self-heal option** through their utility slot. This ensures teams drafted without a dedicated healer (Primalist or Diviner) can still sustain themselves, at the cost of not picking a more offensive or tactical utility ability.

---

## 6. Ability System

### Ability Sources Per Hero

| Source | Abilities | Total |
|--------|-----------|-------|
| Primary Weapon | At-Will + Power + Ultimate | 3 |
| Secondary Weapon | At-Will + Power + Ultimate | 3 |
| Utility Item | 2 abilities from tree | 2 |
| Class Skill Tree | Passives and stat boosts | Varies |
| **Total Active Abilities** | | **8** |

Across a full team of 3 heroes, the player has access to **24 active abilities** in combat.

### Ability Tiers

**At-Will**
- The "basic attack" of each weapon.
- Reliable and low-cost (little to no resource expenditure).
- Usable every turn without restriction.
- Designed to always be a viable option even when resources are depleted.

**Power / Spell**
- The "power attack" of each weapon.
- Significantly more effective than at-wills.
- Costs meaningful resources (durability or mana depending on class).
- Core of moment-to-moment tactical decisions: when to spend resources for bigger impact.

**Ultimate**
- Extremely powerful ability with high-impact effects.
- Costs a large amount of resources to use.
- Additionally limited to a **fixed number of uses per mission** (cannot be recovered without resupply).
- Mission-defining moments: using an ultimate at the right time can turn a fight.

### Resource Systems
- Resources vary by class (durability for martial classes, mana for casters, etc.).
- Resources are managed per-mission. Running out forces reliance on at-wills.
- Ultimates have a hard use cap per mission on top of resource costs.

---

## 7. Skill Trees

### 7.1 Gear Skill Trees

Every gear piece (weapons and utility item) has its own **small skill tree** that determines what abilities the player has access to.

**Structure:**
- Trees are organized into **lines** (horizontal rows).
- Each line presents a choice between **a few options**.
- The player selects **exactly one option per line**.
- Lines unlock progressively as the hero levels up.

**Weapon Skill Trees (3 lines each):**

| Line | Determines | Description |
|------|-----------|-------------|
| Line 1 | At-Will ability | Choose which basic attack this weapon provides |
| Line 2 | Power / Spell | Choose which power attack this weapon provides |
| Line 3 | Ultimate ability | Choose which ultimate this weapon provides |

Each of the 4 weapons a class has access to (2 primary + 2 secondary) has its own unique skill tree with different ability options on each line.

**Utility Skill Tree:**
- Same line-based structure as weapon trees.
- Unlocks **2 abilities** for the hero.
- Includes a self-heal option available to every class.

**Starting State:**
- At level 1, the **first line** (at-will) on all weapon trees is fully unlocked. Players can freely swap their at-will choice.
- The first line of the utility tree is also unlocked from the start.
- Deeper lines unlock as the hero gains levels.

### 7.2 Class Skill Trees

Each class has a **main skill tree** separate from gear trees. This tree provides **passive effects and stat boosts** (not active abilities) and defines the hero's identity beyond their gear.

**Base Tree:**
- Available from level 1.
- Organized into lines with choices (same structure as gear trees).
- All classes have the **same number of lines** in their base tree.
- The player **must select a perk from each line** to unlock the next line below it.
- Nodes include: passive effects, stat increases, class-defining mechanics.
- Completing the base tree is required to unlock specialization.

**Specialization Tree:**
- Unlocks at **level 10** when the base tree is complete.
- The player **chooses one specialization** from multiple options per class.
- Spec trees are more focused on **specific playstyles** than the generalist base tree.
- Same line-based structure as the base tree.
- Same number of lines across all spec trees.

**Examples of Specialization Direction:**
- Dexterous could spec into a trapper (crowd control via summoned traps) or pure assassin (maximum single-target burst).
- Primalist could spec into nature summoner, healer, or elemental warrior.
- These are illustrative - specific specs are not yet designed.

---

## 8. Progression System

### Level Progression (1-20)

| Level Range | Milestone | What Opens Up |
|-------------|-----------|---------------|
| 1 | Starting state | At-will lines unlocked on all weapons. First utility line unlocked. Base class tree available. |
| 2-9 | Core progression | Earn XP from missions. Unlock deeper weapon/utility tree lines. Invest perk points in base class tree. |
| 10 | Specialization | Base tree complete. Choose a specialization. Spec tree opens. |
| 11-19 | Advanced progression | Invest in spec tree. Complete deeper gear tree lines. Refine builds. |
| 20 | Max level | Overclock slot unlocks on each of the hero's 4 weapons. |

### Overclocks (Post-Max-Level)

- Unlocked at level 20 on a per-weapon basis.
- Each weapon has a list of **upgrade modules** (overclocks) the player can research/craft.
- Each overclock **vastly alters the weapon's gameplay and strategy**.
- Overclocks are the endgame build-defining system - they turn good builds into specialized powerhouses.
- Players research and create overclocks using resources earned from missions.
- Inspired directly by Deep Rock Galactic's overclock system.

### Investment Strategy

The game does not force the player into any particular investment pattern:
- **Focused approach:** Main 3 heroes, level them quickly, reach endgame faster.
- **Broad approach:** Invest in all 6 heroes, slower individual progress but more draft flexibility.
- The game naturally slows progression for the broad approach (XP and resources are finite per mission) but does not penalize it.

---

## 9. Missions

### Procedural Generation
- Mission maps are **procedurally generated** to match the biome they take place in.
- Each biome has distinct visual themes, terrain features, and tactical implications.

### Biomes
Biomes determine the map's visual style, terrain layout, and the types of enemies encountered:
- Forest
- Frost
- Lava
- Mountain
- Cave
- Crypt
- (Additional biomes may be added)

### Objectives
Missions have varying objectives inspired by Fire Emblem's mission variety. The objective type is the **primary driver of team composition decisions**:

| Objective Type | Description | Tactical Emphasis |
|----------------|-------------|-------------------|
| Rout the Enemy | Defeat all enemies on the map | Sustained damage, AoE, endurance |
| Reach a Tile | Get a hero to a specific tile within a time limit | Mobility, survivability, path clearing |
| Kill Target | Eliminate a specific high-priority enemy (boss) | Single-target burst, DPR, survivability |
| Escort / Defend | Protect an NPC or location | Tankiness, healing, crowd control, positioning |
| Survival | Survive for a set number of turns | Sustain, healing, defensive abilities, AoE |
| Obtain Items | Retrieve specific items from the map | Mobility, versatility, split-team capability |

### Draft Incentives
Multiple factors influence team composition:
1. **Objective type** - The most important factor. Escort missions favor healers and tanks. Boss fights favor burst damage.
2. **Biome / enemy types** - Different enemies have different resistances and behaviors.
3. **Damage type matchups** - Enemies in frosty biomes may be weak to fire (favoring the Arcanist), etc.
4. **Map layout** - Tight corridors favor AoE and choke control. Open maps favor mobility and range.

The goal is that all 20 possible team compositions (choose 3 from 6) are viable, but different missions naturally favor different compositions.

---

## 10. Summoning

Summoning is distributed across multiple classes rather than concentrated in a dedicated summoner class.

### Summoning Classes
- **Primalist** - Nature summons (beasts, nature elementals)
- **Occultist** - Undead summons
- **Dexterous** (Trapper spec) - Summons traps for crowd control (not traditional summons but functions similarly as board presence)

### Summoned Unit Rules
- Summoned minions are **player-controlled** (similar to D&D 5e).
- They act as full grid units: they occupy tiles, block movement, and can absorb damage.
- Summoning is inherently powerful in grid-based tactical combat because any summoned unit doubles as a **meat shield** that protects player heroes.
- Balancing summoning is an ongoing design challenge. The strength of summoned units as blockers means they must be balanced through cost, duration, fragility, or other limitations.

---

## 11. Out-of-Mission Systems

### Main Menu / Hub
- Hero advancement and build management
- Loadout assembly and modification
- Skill tree investment (class trees and gear trees)
- Overclock research and crafting (post-level-20)
- Mission selection and draft phase

### Tutorial
- A guided tutorial is planned to walk new players through:
  - The draft system
  - Each class's basics
  - Combat mechanics (movement, AP, abilities)
  - Loadout assembly
- Specifics of the tutorial are not yet designed.

---

## Appendix: Design Principles

1. **Depth over accessibility.** Complexity is a feature, not a problem. The target audience wants hundreds of hours of build crafting and tactical decision-making.
2. **Gradual complexity unfolding.** Despite the depth, the game reveals systems progressively through leveling. Level 1 heroes have limited options. Full build complexity only emerges at level 20+.
3. **All compositions viable.** No team of 3 should be unplayable. Different compositions excel at different mission types.
4. **Class identity through feel.** Classes must feel distinct to play, not just have different numbers. The Martial and Dexterous are both physical fighters but attract completely different player mentalities.
5. **Meaningful choices everywhere.** Every skill tree line, every loadout decision, every draft pick should involve a real trade-off.
6. **DRG loadout philosophy.** Weapons define your abilities. Skill trees customize those abilities. Overclocks redefine them. The player builds their hero, they don't just level it.
