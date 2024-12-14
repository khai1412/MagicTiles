namespace BaseDuet.Scripts.Characters
{
    public class CharacterModel
    {
        public float Speed     { get; private set; }
        public float PositionY { get; private set; }
        
        public CharacterModel(float speed, float positionY)
        {
            this.Speed     = speed;
            this.PositionY = positionY;
        }
    }
}