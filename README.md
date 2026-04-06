# 🎮 Top-Down Survival Game
**Game Algo Project**  
**Sherry Binti Hasan (243UC247P2)**  

A 2D top-down action survival game developed in **Unity 6**.

Players must use precise movement and tactical positioning to collect coins while evading relentless monsters and an intelligent Boss AI.

---

## 🛠 Technical Specifications
- **Engine:** Unity 6 (6000.3.9f1)
- **Recommended Resolution:** QHD (2560 × 1440)

---

## 📖 Game Overview
The player starts with **3 Life Points**.

Your goal is to navigate dangerous environments and collect glowing hearts / coins.

If health reaches zero, the game ends immediately.

---

## 🗺 Level Design

### ❤ Level 1: The Swarm
Multiple monster spawners automatically generate enemies.

**Objective:**  
Collect **5 coins** to progress.

**Features:**
- Increasing difficulty over time
- Enemies spawn only on water tiles
- Pathfinding + flocking AI

---

### ❤ Level 2: The Boss Trial
A tactical boss fight with phase-based AI.

**Boss Progression**
- **0–2 Hearts:** Passive
- **3+ Hearts:** Chase mode
- **Final:** 10-second survival collapse phase

---

## ⚙ Core Mechanics

### Player Movement
- Rigidbody2D movement
- Smooth top-down controls
- 3 HP health system

---

### AI & Flocking System
- Separation
- Alignment
- Cohesion

Enemy FSM:
- Idle
- Chase
- Attack

Boss FSM:
- Observer
- Predictor
- Rage
- Collapse

---

### Adaptive Spawner
Dynamic monster spawning system:
- Safe-zone punishment
- Water-tile legal spawning
- Distance-based spawning

---

## 🧠 Features
- FSM enemy AI
- Boss predictive movement
- 2D NavMesh pathfinding
- Flocking system
- Director AI spawning

---

## 🎮 Built With
- Unity 6
- C#
- NavMeshPlus
- Rigidbody2D Physics

---

## 🙏 Acknowledgements
Tutorials and assets referenced from YouTube, NavMeshPlus, Kenney, and Itch.io creators.
