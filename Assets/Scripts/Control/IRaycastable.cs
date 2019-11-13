namespace TWQ.Control
{
    public interface IRaycastable
    {
        CursorType GetCursorType();
        bool HandleRaycast(PlayerControler callingControler);
    }
}

