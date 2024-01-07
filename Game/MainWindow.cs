using Game.Shaders;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Graphics.OpenGL4;

namespace Game;

public class MainWindow : GameWindow
{
    private Shader _shader;
    private int _vertexArrayObject;
    private int _vertexBufferObject;
    private int _elementBufferObject;
    private Matrix4 _view;
    private Matrix4 _projection;
    private double _time;

    private float[] _vertices = new float[]
    {
        .5f, -5f, -.5f,
        .5f, -.5f, .5f,
        -.5f, -.5f, .5f,
        -.5f, -.5f, -.5f,

        .5f, .5f, -.5f,
        .5f, .5f, .5f,
        -.5f, .5f, .5f,
        -.5f, .5f, -.5f
    };

    private uint[] _indices = new uint[]
    {
        0, 1, 2,
        2, 3, 0,

        4, 6, 6,
        6, 7, 4,

        0, 4, 5,
        5, 1, 0,

        1, 5, 6,
        6, 2, 1,

        2, 3, 7,
        7, 6, 1,

        3, 0, 4,
        4, 7, 3
    };

    public MainWindow()
        : base(GameWindowSettings.Default, NativeWindowSettings.Default)
    {
        CenterWindow(new Vector2i(1000, 1000));
        _shader = new Shader("/home/fyodor/RiderProjects/Minecraft/Game/Shaders/shader.vert", "/home/fyodor/RiderProjects/Minecraft/Game/Shaders/shader.fraq");
    }


    protected override void OnLoad()
    {
        _vertexArrayObject = GL.GenVertexArray();
        GL.BindVertexArray(_vertexArrayObject);

        _vertexBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ArrayBuffer, _vertexBufferObject);
        GL.BufferData(BufferTarget.ArrayBuffer, _vertices.Length * sizeof(float), _vertices,
            BufferUsageHint.StreamDraw);
        GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);

        _elementBufferObject = GL.GenBuffer();
        GL.BindBuffer(BufferTarget.ElementArrayBuffer, _elementBufferObject);
        GL.BufferData(BufferTarget.ElementArrayBuffer, _indices.Length * sizeof(uint), _indices,
            BufferUsageHint.StreamDraw);
        _view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);
        _projection =
            Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), Size.X / (float)Size.Y, 0.1f,
                100.0f);
        base.OnLoad();
    }


    protected override void OnRenderFrame(FrameEventArgs args)
    {
        _time += 4.0 * args.Time;

        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        GL.BindVertexArray(_vertexArrayObject);

        _shader.Use();

        var model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(_time));

        _shader.SetMatrix4("model", model);
        _shader.SetMatrix4("view", _view);
        _shader.SetMatrix4("projection", _projection);

        GL.DrawElements(PrimitiveType.Triangles, _indices.Length, DrawElementsType.UnsignedInt, 0);

        SwapBuffers();
        base.OnRenderFrame(args);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
    }

    protected override void OnResize(ResizeEventArgs e)
    {
        base.OnResize(e);
        GL.Viewport(0, 0, e.Width, e.Height);
    }
}