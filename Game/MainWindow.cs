using Game.Shaders;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;

namespace Game;

public class MainWindow : GameWindow
{
    private int _vertexBufferObject;
    private Shader _shader;
    private int _vertexArrayObject;

    private readonly float[] _vertices = new[]
    {
        0f, 0.5f, 0,
        -0.5f, 0, 0,
        0.5f, 0, 0
    };

    public MainWindow()
        : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        CenterWindow(new Vector2i(1000, 500));
        _shader = new Shader("/home/fyodor/RiderProjects/Minecraft/Game/Shaders/shader.vert",
            "/home/fyodor/RiderProjects/Minecraft/Game/Shaders/shader.fraq");
    }

    protected override void OnLoad()
    {
        base.OnLoad();
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
            BufferUsageHint.StaticDraw);

        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);

        GL.ClearColor(Color4.Aqua);
        GL.Clear(ClearBufferMask.ColorBufferBit);
        _shader.Use();


        GL.BindVertexArray(_vertexArrayObject);
        GL.DrawArrays(PrimitiveType.Triangles, 0, 3);
        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }
}