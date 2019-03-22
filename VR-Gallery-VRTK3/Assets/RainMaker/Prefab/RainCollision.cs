//
// Rain Maker (c) 2016 Digital Ruby, LLC
// http://www.digitalruby.com
//

using UnityEngine;
using System.Collections.Generic;

namespace DigitalRuby.RainMaker
{
    public class RainCollision : MonoBehaviour
    {
        private static readonly Color32 color = new Color32(255, 255, 255, 255);
        private readonly List<ParticleCollisionEvent> collisionEvents = new List<ParticleCollisionEvent>();

        public ParticleSystem RainExplosion;
        public ParticleSystem RainParticleSystem;
		public float collisionRadius;

		Camera camera;

        private void Start()
        {
			
        }

        private void Update()
        {

        }

        private void Emit(ParticleSystem p, ref Vector3 pos)
        {
            int count = UnityEngine.Random.Range(2, 5);
            while (count != 0)
            {
                float yVelocity = UnityEngine.Random.Range(1.0f, 3.0f);
                float zVelocity = UnityEngine.Random.Range(-2.0f, 2.0f);
                float xVelocity = UnityEngine.Random.Range(-2.0f, 2.0f);
                const float lifetime = 0.75f;// UnityEngine.Random.Range(0.25f, 0.75f);
                float size = UnityEngine.Random.Range(0.05f, 0.1f);
                ParticleSystem.EmitParams param = new ParticleSystem.EmitParams();
                param.position = pos;
                param.velocity = new Vector3(xVelocity, yVelocity, zVelocity);
                param.startLifetime = lifetime;
                param.startSize = size;
                param.startColor = color;
                p.Emit(param, 1);
                count--;
            }
        }

        private void OnParticleCollision(GameObject obj)
        {
			if (!camera) {
				camera = Camera.main;
			}
			if (RainExplosion != null && RainParticleSystem != null)
            {
                int count = RainParticleSystem.GetCollisionEvents(obj, collisionEvents);
                for (int i = 0; i < count; i++)
                {
                    ParticleCollisionEvent evt = collisionEvents[i];
                    Vector3 pos = evt.intersection;
					Vector3 campos = new Vector3(camera.transform.position.x, pos.y, camera.transform.position.z);
					if (Vector3.Distance(pos, campos) < collisionRadius) {
						Emit(RainExplosion, ref pos);
					}
                }
            }
        }
    }
}