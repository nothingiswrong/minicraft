using Game.Shaders;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;

namespace Game;

public class MainWindow : GameWindow
{
    private int _vertexBufferObject;
    private readonly Shader _shader;
    private int _vertexArrayObject;
    private int _elementBufferObject;

    private uint[] _indices = new uint[]
    {
        0, 1, 2,
        2, 3, 0
    };

    private readonly float[] _vertices = new float[]
    {
        0, 0, 0,
        0, .5f, 0,
        .5f, .5f, 0,
        .5f, 0, 0
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
        //VBO
        base.OnLoad();
        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
            BufferUsageHint.StaticDraw);
        //VAO
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        //EBO 
        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices,
            BufferUsageHint.StaticDraw);
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
        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);
        SwapBuffers();
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }
}