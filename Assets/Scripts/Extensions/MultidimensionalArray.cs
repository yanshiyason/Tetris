using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Tetris.Extensions;
using System.Linq;

namespace Tetris {
    namespace Extensions {
        public static class ExtendedMultidimensionalArray {
            public static void SetGridValue(this bool[,] grid, bool value, Vector3 v) {
                grid.SetValue(value, (int)v.x, (int)v.y);
            }

            public static void SetGridValues(this bool[,] grid, bool value, Transform group, Vector3 nextPosition)
            {
                IEnumerable<Transform> transforms = group.transform.Cast<Transform>();
                foreach (var t in group)
                {
                    var next = t.position + nextPosition;
                    grid.SetGridValue(true, next);
                }
            }
        }
    }
}