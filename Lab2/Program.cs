namespace StrategyGame
{
    internal class Program
    {
        public interface IAttacker
        {
            void Attack(Unit unit);
        }
        public interface IMoveable
        {

        }
        public class GameObject
        {
            private int _id;
            private string _name;
            private int _x;
            private int _y;

            public GameObject(int id, string name, int x, int y)
            {
                _id = id;
                _name = name;
                _x = x;
                _y = y;
            }

            public int GetId()
            {
                return _id;
            }
            public string GetName()
            {
                return _name;
            }
            public int GetX()
            {
                return _x;
            }
            public int GetY()
            {
                return _y;
            }
        }

        public class Unit : GameObject
        {
            private bool _IsAlive;
            private float _hp;

            public Unit(int id, string name, int x, int y, float hp) :base(id, name, x, y)
            {
                _hp = hp;
                _IsAlive = hp > 0;
            }

            public bool IsAlive()
            {
                return _IsAlive;
            }
            public float GetHp()
            {
                return _hp;
            }

            public void receiveDamage(float damage)
            {
                if (_IsAlive == false)
                {
                    return;
                }
                _hp -= damage;
                Console.WriteLine(GetName() + " получил " + damage + " урона! Осталось здоровья: " + _hp);

                if(_hp <= 0)
                {
                    _IsAlive = false;
                    _hp = 0;
                    Console.WriteLine(GetName() + " погиб в бою!");
                }
            }
        }

        public class Archer : Unit, IAttacker, IMoveable
        {
            private float _damagePower;

            public Archer(int id, string name, int x, int y, float hp, float damagePower) : base(id, name, x, y, hp)
            {
                _damagePower = damagePower;
            }

            public void Attack(Unit target)
            {
                Console.WriteLine("Лучник " + GetName() + " стреляет в " + target.GetName() + "!");
                target.receiveDamage(_damagePower);
            }
        }

        public class Building : GameObject
        {
            private bool _IsBuilt;

            public Building(int id, string name, int x, int y, bool IsBuilt) : base(id, name, x, y)
            {
                _IsBuilt = IsBuilt;
            }
            public bool IsBuilt()
            {
                return _IsBuilt;
            }
        }

        public class Fort : Building, IAttacker
        {
            private float _cannonDamage;

            public Fort(int id, string name, int x, int y, bool IsBuilt, float CannonDamage) : base(id, name, x, y, IsBuilt)
            {
                _cannonDamage = CannonDamage;
            }

            public void Attack(Unit target)
            {
               if(IsBuilt() == false)
               {
                    Console.WriteLine("Крепость " + GetName() + " еще не достроена. Пушки не могут стрелять!");
                    return;
               }
                Console.WriteLine("Крепость " + GetName() + " делает залп по " + target.GetName() + "!");
                target.receiveDamage(_cannonDamage);
            }
        }
        
        public class MobileHome : Building,IMoveable
        {
            public MobileHome(int id, string name, int x, int y, bool IsBuilt) : base(id, name, x, y, IsBuilt)
            {

            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Hello, World!");
        }
    }
}
