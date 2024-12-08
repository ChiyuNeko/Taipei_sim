using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = System.Random;
using System.Threading.Tasks;

namespace Project.Scripts.Fractures
{
    public class FractureThis : MonoBehaviour
    {
        public bool preFracture = false;

        [SerializeField] private Anchor anchor = Anchor.Bottom;
        [SerializeField] private int chunks = 500;
        [SerializeField] private float density = 50;
        [SerializeField] private float internalStrength = 100;

        [SerializeField] private Material insideMaterial;
        [SerializeField] private Material outsideMaterial;

        private Random rng = new Random();


        void Start()
        {
            
            if (preFracture)
            {
                gameObject.layer = LayerMask.NameToLayer("FrozenChunks");
                StartFractureGameobject();
            }
            outsideMaterial = GetComponent<MeshRenderer>().materials[0];
            insideMaterial = GetComponent<MeshRenderer>().materials[0];
        }

        void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag == "hug-IceBall")
            {
                gameObject.layer = LayerMask.NameToLayer("FrozenChunks");
                Destroy(collision.gameObject);
                StartFractureGameobject();
            }
        }

        void StartFractureGameobject()
        {
            // Fracture the game object
            FractureGameobject();

            gameObject.SetActive(false);
        }

        public  ChunkGraphManager FractureGameobject()
        {
            var seed = rng.Next();
            return Fracture.FractureGameObject(
                gameObject,
                anchor,
                seed,
                chunks,
                insideMaterial,
                outsideMaterial,
                internalStrength,
                density
            );
        }
    }
}
