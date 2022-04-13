using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimientoCaracola : MonoBehaviour
{
    float _velocidadX;
    float _impulso;
    float _aceleracion = 250f;
    float _velocidadMaxima = 14f;
    float _longitudRaycast = 0.4f;
    float _tamañoRaycast = 0.2f;
    bool _enMovimiento = false;
    public Animator _anim;
    void Start()
    {
        _anim.GetComponent<Animator>();
    }

    public bool moviendose()
    {
        return this._enMovimiento;
    }

    public void moviendose(bool _moviendo)
    {
        this._enMovimiento = _moviendo;
    }

    void FixedUpdate()
    {
        var _rbCaracola = GetComponent<Rigidbody2D>();

        if (_rbCaracola != null)
        {
            _velocidadX = _rbCaracola.velocity.x;

            if (_velocidadX < 0)
            {
                _rbCaracola.AddForce(new Vector2(-_aceleracion, 0));
            } else if (_velocidadX > 0) {
                _rbCaracola.AddForce(new Vector2(_aceleracion, 0));
            }

            if (_velocidadX != 0)
            {
                _enMovimiento = true;
                _impulso = _velocidadX;
            } else {
                _enMovimiento = false;
            }

            _anim.SetBool("moviendose", _enMovimiento);

            Vector3 _velocidad = _rbCaracola.velocity;
            _velocidad.x = Mathf.Clamp(_velocidad.x, -_velocidadMaxima, _velocidadMaxima);
            _rbCaracola.velocity = _velocidad;

            var _raycast = transform.Find("ray");

            var _inicioRaycast = new Vector2(_raycast.transform.position.x, _raycast.transform.position.y + _tamañoRaycast);
            var _raycastDerecha = new Vector2(_raycast.transform.position.x + _longitudRaycast, _raycast.transform.position.y + _tamañoRaycast);
            var _raycastIzquierdo = new Vector2(_raycast.transform.position.x - _longitudRaycast, _raycast.transform.position.y + _tamañoRaycast);

            RaycastHit2D[] _golpeDerecha = Physics2D.LinecastAll(_inicioRaycast, _raycastDerecha);
            RaycastHit2D[] _golpeIzquierdo = Physics2D.LinecastAll(_inicioRaycast, _raycastIzquierdo);

            Debug.DrawLine(_inicioRaycast, _raycastDerecha, Color.red);
            Debug.DrawLine(_inicioRaycast, _raycastIzquierdo, Color.red);

            foreach (RaycastHit2D _golpe in _golpeDerecha)
            {
                var _collider = _golpe.collider;

                if (_collider != null)
                {
                    if (_collider.gameObject.tag == "pared")
                    {
                        _rbCaracola.velocity = new Vector2(-Mathf.Abs(_rbCaracola.velocity.x), _rbCaracola.velocity.y);
                        _rbCaracola.AddForce(new Vector2(-_velocidadMaxima, 0));
                    }
                }
            }

            foreach (RaycastHit2D _golpe in _golpeIzquierdo)
            {
                var _collider = _golpe.collider;

                if (_collider != null)
                {
                    if (_collider.gameObject.tag == "pared")
                    {
                        _rbCaracola.velocity = new Vector2(Mathf.Abs(_rbCaracola.velocity.x), _rbCaracola.velocity.y);
                        _rbCaracola.AddForce(new Vector2(_velocidadMaxima, 0));
                    }
                }
            }
        }
    }
}
