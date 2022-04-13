using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trigger : MonoBehaviour
{
    public void OnTriggerEnter2D(Collider2D _objeto)
    {
        var _control = transform.parent.GetComponent<movimientoJugador>();

        if (_objeto.gameObject.tag == "caracola")
        {
            var _caracola = _objeto.gameObject.GetComponent<movimientoCaracola>();

            if (_caracola.moviendose())
            {
                var _posicion = _control.transform.Find("pie");

                var _raycastInicial = _posicion.transform.position;
                var _raycastArriba = new Vector2(_posicion.transform.position.x, _posicion.transform.position.y);

                RaycastHit2D[] _golpes = Physics2D.LinecastAll(_raycastInicial, _raycastArriba);

                foreach (RaycastHit2D _golpe in _golpes)
                {
                    var _collider = _golpe.collider;

                    if (_collider != null)
                    {
                        if (_collider.gameObject.tag == "caracola")
                        {
                            var _rbJugador = _control.GetComponent<Rigidbody2D>();
                            _rbJugador.velocity = new Vector2(_rbJugador.velocity.x, 12f);

                            _caracola.moviendose(false);
                            var _rb = _caracola.GetComponent<Rigidbody2D>();
                            _rb.velocity = new Vector2();
                        }
                    }
                }
            }
            else
            {
                if (Input.GetKey(KeyCode.Z))
                {
                    _control.agarrarCaracola();
                }
                else
                {
                    _control.patearCaracola();
                }
            }
        }
    }
}
