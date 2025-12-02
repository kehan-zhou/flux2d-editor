# Flux2DEditor

**Flux2DEditor** is a modular, extensible, and architecture-first **2D Editor Engine** built in **C# / .NET 8**.

It is designed as a foundation for building CAD-like editors, 2D drawing tools, visualization systems, and custom domain-specific editors.

This project uses a clean, multi-layered architecture:
- **Core** - shapes, geometry, commands, editing logic
- **Render** - backend rendering system (GDI+ by default)
- **WinForms** - UI layer powered by a custom **Viewport** component

# ✨ Features

## 🖱️Interactive Editing Tools
 - **Selection Tool**
 - **Rectangle Drawing Tool** (click-and-drag to create shapes)
 - **Circle Drawing Tool** (click-and-drag to create shapes)
 - Resize using control handles
 - Drag to move shapes

## 🧩 Modular Architecture
- Clean separation of **Core**, **Render**, **WinForms UI**
- Shapes implement `IShape` and work in any environment (not tied to WinForms)
- Rendering implemented through `IRenderContext` and `IRenderer`

## 🖼️ Advanced Viewport
- High-performance zooming & panning
- Screen ⇄ World coordinate conversion
- Smooth interactive drawing
- Controlled entirely by the UI's Viewport layer

## 🔁 Undo / Redo System
- Command-base architecture
- Supports Add, Move, Resize, Copy, Cut, Paste, Delete

## 🛠️ Easily Extensible
- Add custom shapes
- Add new drawing tools
- Swap rendering backend (GDI → Skia → Direct2D → OpenGL)
- Integrate into other editors or CAD applications

# 🧰 Technologies Used

- **C# / .NET 8.0**
- **WinForms** UI framework
- **Custom ViewportControl** for zoom/pan rendering pipeline

# ⚙️ Usage

1. Clone the repository:
    ```bash
    git clone https://github.com/kehan-zhou/Flux2DEditor.git
    ```
2. Open `Flux2DEditor.sln` in Visual Studio.
3. Build the solution.
4. Start the `Flux2DEditor.WinForms` project.

# 🔩 Related Projects

- [ViewportControl](https://github.com/kehan-zhou/viewport-control) --- The custom control used by the WinForms layer for 2D viewport zooming, panning, and coordinate transforms.

# 📄 License

This project is licensed under the MIT License.

You're free to use, modify, and distribute this control in personal and commercial projects.

See the [LICENSE](LICENSE) file for full details.


