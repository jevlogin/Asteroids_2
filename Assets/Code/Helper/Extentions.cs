using System;
using UnityEngine;


namespace WORLDGAMEDEVELOPMENT
{
    public static class Extentions
    {
        public static GameObject AddSprite(this GameObject gameObject, Sprite sprite)
        {
            var component = gameObject.GetOrAddComponent<SpriteRenderer>();
            component.sprite = sprite;
            return gameObject;
        }

        public static GameObject AddRigidbody2D(this GameObject gameObject)
        {
            gameObject.GetOrAddComponent<Rigidbody2D>();
            return gameObject;
        }

        public static GameObject AddCircleCollider2D(this GameObject gameObject)
        {
            gameObject.GetOrAddComponent<CircleCollider2D>();
            return gameObject;
        }

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component
        {
            var result = gameObject.GetComponent<T>();
            if (!result)
            {
                result = gameObject.AddComponent<T>();
            }
            return result;
        }

        public static string GetStringRub(this int amount)
        {
            if (amount < 0)
            {
                throw new ArgumentException("Не бывает таких рублей.");
            }

            if (amount % 10 == 1 && amount % 100 != 11)
            {
                return ManagerName.TEXT_SCORE_PREFIX_ONE;
            }
            else if ((amount % 10 >= 2 && amount % 10 <= 4) && (amount % 100 < 12 || amount % 100 > 14))
            {
                return ManagerName.TEXT_SCORE_PREFIX_TWO;
            }
            else
            {
                return ManagerName.TEXT_SCORE_PREFIX_FIVE;
            }
        }
    }
}