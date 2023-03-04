namespace WDCommunication.WDTcp
{
    public enum EnumServerStateType { 停止 = 0, 运行 = 1 }
    /// <summary>
    /// 
    /// </summary>
    public class StateEventArgs : EventArgs
    {
        public StateEventArgs(EnumServerStateType state)
        {
            State = state;
        }

        public EnumServerStateType State { get; }
    }
}
