
namespace IMDemo.Core.UI
{
    public class MenuItem
    {
        public string Name;
        public Action OnClick;
        public List<MenuItem> Childs;
    }
}