# Flux2DEditor

**Flux2DEditor** is a minimal **reference architecture** for implementing interaction in 2D editors.

It focuses on the problems UI frameworks don't solve:
- world <-> screen coordinates
- hit-testing geometry
- selecting shapes precisely
- dragging control handles
- snapping
- tool state machines
- undoable editor operations

Instead of coupling behavior to a UI framework, Flux2DEditor models an editor as a system of:
> Document (data) + Tools (intent) + Commands (history) + Camera (view)

The WinForms application included in this repository is only a demonstration host.

---

## 🔦 What this project is (and is not)
This is **not** a widget toolkit or drawing library.

It is a reference interaction model used by editors such as CAD tools, level editors, diagram editors, and visualization tools.

> 🚧 **Project Status:**  
> Core interaction model is stable.
> Tools and features are added gradually as real editor scenarios are explored.

---

## 🧱 Architecture Overview

Flux2DEditor follows a **clean, layered architecture**, inspired by Clean Architecture / DDD principles:

```txt
Domain
├─ Geometry (Vector, BoundingBox, etc.)
├─ Shapes (Rectangle, ShapeId, etc.)
└─ Scene Model

Application
├─ Editor Controller
├─ Tools (Selection, Marquee, etc.)
├─ Commands (Undo / Redo)
└─ Interaction Logic

Presentation.WinForms
├─ WinForms UI
├─ GDI+ Renderer
└─ Viewport Integration
```

### Architectural Principles

- **Domain is UI-agnostic**
- **Editor logic is decoupled from rendering**
- **All user interactions are modeled explicitly**
- **Undo / Redo is command-driven**
- Designed for **long-term extensibility**, not quick demos

## 🧰 Technologies

- Runtime: .NET 8
- Host: WinForms (reference implementation)
- Rendering: GDI+ (replaceable)
- Custom [ViewportControl](https://github.com/kehan-zhou/viewport-control)

## 🚀 Getting Started

1. Clone the repository.
2. Open `Flux2DEditor.sln` in Visual Studio.
3. Build the solution.
4. Start the `Flux2DEditor.Presentation.WinForms`.

## 🔩 Related Projects

- [ViewportControl](https://github.com/kehan-zhou/viewport-control) --- A reusable WinForms control providing zooming, panning, and coordinate transforms.

## 📄 License

This project is licensed under the MIT License.

You're free to use, modify, and distribute this control in personal and commercial projects.

See the [LICENSE](LICENSE) file for full details.


