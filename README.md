# Flux2DEditor

**Flux2DEditor** is an **architecture-first, extensible 2D editor engine** written in **C# / .NET 8**.

It is designed as a long-term foundation for building **CAD-like editors**, **2D drawing tools**, **visualization systems**, and **domain-specific editors**, with a strong focus on **clean architecture**, **editor interaction modeling**, and **maintainability**.

> 🚧 **Project Status:**  
> Flux2DEditor is under **active development**.  
> Core editor interactions and architecture are stable, while advanced tools and features are continuously evolving.

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

- **C# / .NET 8.0**
- **WinForms**
- **GDI+ rendering**
- **Custom ViewportControl**

## 🚀 Getting Started

1. Clone the repository:
    ```bash
    git clone https://github.com/kehan-zhou/Flux2DEditor.git
    ```
2. Open `Flux2DEditor.sln` in Visual Studio.
3. Build the solution.
4. Start the `Flux2DEditor.Presentation.WinForms`.

## 🔩 Related Projects

- [ViewportControl](https://github.com/kehan-zhou/viewport-control) --- A reusable WinForms control providing zooming, panning, and coordinate transforms.

## 📄 License

This project is licensed under the MIT License.

You're free to use, modify, and distribute this control in personal and commercial projects.

See the [LICENSE](LICENSE) file for full details.


