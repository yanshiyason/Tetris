using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Tetris.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace Tetris {
    namespace Extensions {
        public static class ExtendedMultidimensionalArray {
            public static void SetGridValue (this bool[, ] grid, bool value, Vector3 v) {
                grid.SetValue (value, (int) v.x, (int) v.y);
            }

            // TODO: Maybe remove
            public static void SetGridValues (this bool[, ] grid, bool value, Transform group, Vector3 nextPosition) {
                IEnumerable<Transform> transforms = group.transform.Cast<Transform> ();
                foreach (var t in transforms) {
                    var next = t.position + nextPosition;
                    grid.SetGridValue (true, next);
                }
            }
        }
    }
}