namespace BeeEngine;

public enum CursorState
{
    Normal, Hidden, Disabled
}

public static class Cursor
{
    private static CursorState _cursorState = CursorState.Normal;
    public static void SetCursorState(CursorState state)
    {
        switch (state)
        {
            case CursorState.Normal:
                WindowHandler.Instance.ShowCursor();
                break;
            case CursorState.Hidden:
                WindowHandler.Instance.HideCursor();
                break;
            case CursorState.Disabled:
                WindowHandler.Instance.DisableCursor();
                break;
            default: Log.Error("Invalid Cursor state");
                break;
        }

        _cursorState = state;
    }

    public static CursorState GetCursorState()
    {
        return _cursorState;
    }
}