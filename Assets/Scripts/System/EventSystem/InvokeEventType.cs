public enum InvokeEventType
{
    One,//发送给指定位置
    Two,//发送给当前位置和后续位置
    Three,//发送给除了当前方向以外其他三个位置
    Four, //发送给周围四格
    AllId //发送给所有物体
}