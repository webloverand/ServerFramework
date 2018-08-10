namespace Common
{
    public enum ReasonCode
    {
        None,
        IllegalCharacter,//含有非法字符
        UsernameOrPwdError,//用户名或者密码错误
        RepeatRegister , //重复注册
        DatabaseException , //数据库异常
    }
}
