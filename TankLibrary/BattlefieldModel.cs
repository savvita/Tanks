namespace TankLibrary
{
    public class BattlefieldModel
    {
        /// <summary>
        /// Tanks at the field
        /// </summary>
        public List<TankManModel> Tankmen { get; set; } = new List<TankManModel>();

        /// <summary>
        /// True if the battle has ended otherwise false
        /// </summary>
        public bool IsFinished { get; set; } = false;

        /// <summary>
        /// Raise when the tankman has lost
        /// </summary>
        public event Action<TankManModel>? Lost;

        /// <summary>
        /// Raise when the tankman has won
        /// </summary>
        public event Action<TankManModel>? Won;

        public void OnLost(TankManModel model)
        {
            Lost?.Invoke(model);
        }

        public void OnWon(TankManModel model)
        {
            Won?.Invoke(model);
        }

        public bool IsInPlay { get; set; } = false;

        /// <summary>
        /// Evaluate damages
        /// </summary>
        public void HandleBattle()
        {
            if (!IsInPlay)
            {
                return;
            }

            for (int i = 0; i < Tankmen.Count; i++)
            {
                int lives = 0;

                if (Tankmen[i].Tank == null)
                {
                    continue;
                }

                if (!Tankmen[i].Tank!.IsAlive || !Tankmen[i].Tank!.IsFire)
                {
                    continue;
                }

                if (Tankmen[i].Tank!.Bullet == null)
                {
                    continue;
                }

                for (int j = 0; j < Tankmen.Count; j++)
                {
                    if (Tankmen[j].Tank == null)
                    {
                        continue;
                    }

                    if(Tankmen[i] == Tankmen[j])
                    {
                        continue;
                    }

                    if (Tankmen[j].Tank!.Rectangle.Contains(Tankmen[i].Tank!.Bullet!.Location))
                    {
                        Tankmen[i].Tank!.IsFire = false;
                        Tankmen[i].Tank!.Bullet!.IsFlying = false;

                        Tankmen[j].Tank!.Health -= Tankmen[i].Tank!.Damage;
                        Tankmen[j].Tank!.IsHit = true;

                        if (Tankmen[j].Tank!.Health <= 0)
                        {
                            Tankmen[j].Tank!.IsAlive = false;
                            OnLost(Tankmen[j]);
                        }
                    }

                    if (Tankmen[j].Tank!.IsAlive)
                    {
                        lives++;
                    }
                }

                if (lives == 0)
                {
                    int idx = FindWinner();

                    if (idx != -1)
                    {
                        OnWon(Tankmen[idx]);
                    }
                    IsFinished = true;
                    IsInPlay = false;
                    break;
                }
            }
        }

        /// <summary>
        /// Find last alive
        /// </summary>
        /// <returns>Index of winner if it is founded otherwise -1</returns>
        private int FindWinner()
        {
            for (int i = 0; i < Tankmen.Count; i++)
            {
                if (Tankmen[i].Tank == null)
                {
                    continue;
                }

                if (Tankmen[i].Tank!.IsAlive)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}