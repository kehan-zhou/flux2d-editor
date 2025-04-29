# Flux2DEditor

**Flux2DEditor** is a lightweight, extensible, and highly modular **2D Editor Engine** built on **WinForms (.NET)** using a custom-designed **ViewportControl** component.

It is designed to serve as a flexible foundation for developing specialized 2D editors, visualization tools, CAD-like software, and more.

# ✨ Features

- **Custom 2D Viewport:** Supports zooming, panning, and world/screen coordinate conversion.
- **Flexible Architecture:** Clear module separation for Forms, Core logic, Rendering, Services, and Utilities.
- **Easy to Extend:** Future-friendly project structure for plugins and new editing tools.
- **High Usability:** Designed for creating 2D graphics, object editing, and scene management.

# 🧰 Technologies Used

- C# (.NET 8.0 or later recommended)
- WinForms
- [ViewportControl](https://github.com/kehan-zhou/viewport-control) --- Custom 2D Viewport component

# 🪢 Project Structure

```txt
Flux2DEditor/
├── src/
│   └── Flux2DEditor/
│       ├── Forms/
│       ├── Core/
│       ├── Render/
│       ├── Services/
│       ├── Utils/
│       ├── Resources/
│       ├── Properties/
│       ├── Flux2DEditor.csproj
│       └── Program.cs
├── .gitignore
├── CODE_OF_CONDUCT.md
├── Flux2DEditor.sln
├── LICENSE
└── README.md
```

# ⚙️ Usage

1. Clone the repository:
    ```bash
    git clone https://github.com/kehan-zhou/Flux2DEditor.git
    ```
2. Open `Flux2DEditor.sln` in Visual Studio.
3. Build the solution.
4. Run the application.

# 🔩 Related Projects

- [ViewportControl](https://github.com/kehan-zhou/viewport-control) --- The custom control used for zooming, panning, and rendering.

# 📄 License

This project is licensed under the MIT License.

You're free to use, modify, and distribute this control in personal and commercial projects.

See the [LICENSE](LICENSE) file for full details.


