using Leopotam.EcsLite;
using UnityEngine;

namespace Scripts
{
    public class PointToHex : MonoBehaviour
    {
        [SerializeField] private float d;
        
        private Camera _camera;
        private ViewSettings _viewSettings;
        private Vector3 _mouseWorldPos;
        private FieldService _fieldService;
        private EcsWorld _world;

        private void Start()
        {
            _camera = Camera.main;
        }

        public void Inject(EcsWorld world, FieldService fieldService, ViewSettings viewSettings)
        {
            _fieldService = fieldService;
            _viewSettings = viewSettings;
            _world = world;
        }

        private void Update()
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            var plane = new Plane(Vector3.up, d);
            if (plane.Raycast(ray, out var enter))
            {
                _mouseWorldPos = ray.GetPoint(enter);
            }

            var coordinates = GetHexCoordinates(new Vector2(_mouseWorldPos.x, _mouseWorldPos.z));
            
            ref var request = ref _world.Send<HighlightRequest>();
            request.Target = _fieldService.GetCellEntity(coordinates);
        }
        
        //  ⎡q⎤     ⎡   2/3         0    ⎤   ⎡x⎤
        //  ⎢ ⎥  =  ⎢                    ⎥ × ⎢ ⎥ ÷ size
        //  ⎣r⎦     ⎣  -1/3    sqrt(3)/3 ⎦   ⎣y⎦

        private Coordinates GetHexCoordinates(Vector2 pos)
        {
            var q = (2f / 3 * pos.x) / _viewSettings.HexSize;
            var r = (-1f / 3 * pos.x + Mathf.Sqrt(3) / 3 * pos.y) / _viewSettings.HexSize;
            return RoundCoordinates(new Fractional(q, r));
        }

        private Coordinates RoundCoordinates(Fractional frac)
        {
            var q = Mathf.RoundToInt(frac.q);
            var r = Mathf.RoundToInt(frac.r);
            var s = Mathf.RoundToInt(frac.s);

            var qDiff = Mathf.Abs(q - frac.q);
            var rDiff = Mathf.Abs(r - frac.r);
            var sDiff = Mathf.Abs(s - frac.s);

            if (qDiff > rDiff && qDiff > sDiff)  
                q = -r - s;
            else if (rDiff > sDiff) 
                r = -q - s;
            else 
                s = -q - r;
            return new Coordinates(q, r, s);
        }
    }
}
