namespace Controllers
{
    public class GooGameController : Singleton<GooGameController>
    {
        public void BackToHome()
        {
            GameManager.Instance.BackToHome();
        }
    }
}