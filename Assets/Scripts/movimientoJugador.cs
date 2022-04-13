using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movimientoJugador : MonoBehaviour
{
    float _direccionX;
    float _velocidadX = 5f;
    float _fuerzaSalto = 12f;
    float _fuerzaPatada = 500f;
    public GameObject _objeto;
    bool _agarrarCaracola;
    GameObject _caracola;
    public Transform _mano;
    bool _enSuelo;
    Vector2 _direccion;
    public Transform _pie;
    float _radioPie = 0.03f;
    public Animator _anim;
    public Rigidbody2D _rb;
    public LayerMask _suelo;
    public GameObject _personaje;
    void Start()
    {
        _rb.GetComponent<Rigidbody2D>();
        _anim.GetComponent<Rigidbody2D>();
    }
    
    public void patearCaracola()
    {
        var _rbCaracola = _objeto.GetComponent<Rigidbody2D>();
        var _rb = GetComponent<Rigidbody2D>();
        var _escalaLocal = _rb.transform.localScale;
        _agarrarCaracola = false;

        _rbCaracola.AddForce(new Vector2(_fuerzaPatada * _escalaLocal.x, 0));
    }

    public void agarrarCaracola()
    {
        if (_caracola == null)
        {
            _caracola = _objeto;

            _caracola.transform.parent = this.transform;
            var _posicionMano = _mano.transform.localPosition;
            _agarrarCaracola = true;

            _caracola.transform.localPosition = new Vector2(_posicionMano.x + 0.2f * _mano.localScale.x, _posicionMano.y);

            _anim.SetBool("is_holding", true);
            var _rbCaracola = _objeto.GetComponent<Rigidbody2D>();
            Object.Destroy(_rbCaracola);

            _caracola.layer = (int)LevelManager.capas.tomarCaracola;
        }
    }

    public void soltarCaracola()
    {
        if (_caracola != null)
        {
            _caracola.transform.parent = null;
            _caracola.gameObject.AddComponent<Rigidbody2D>();

            var _rbCaracola = _caracola.GetComponent<Rigidbody2D>();
            _rbCaracola.freezeRotation = true;
            _rbCaracola.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            _rbCaracola.gravityScale = 3;

            patearCaracola();
            _caracola.layer = (int)LevelManager.capas.caracola;
            _anim.SetBool("is_holding", false);
            _agarrarCaracola = false;
            _caracola = null;
        }
    }
    void Update()
    {
        float _movimientoX = 0.0f;
        _direccionX = Input.GetAxis("Horizontal");
        float _direccionY = Input.GetAxis("Jump");
        _direccion = new Vector2(_direccionX, _direccionY);

        _movimientoX = _direccionX * _velocidadX;

        _enSuelo = Physics2D.OverlapCircle(_pie.position, _radioPie, _suelo);

        if (_direccionX > 0) 
        {
            _rb.velocity = (new Vector2(_direccion.x * _velocidadX, _rb.velocity.y));
            _personaje.transform.localScale = new Vector3(1, 1, 1);
        } else if (_direccionX < 0)
        {
            _rb.velocity = (new Vector2(_direccion.x * _velocidadX, _rb.velocity.y));
            _personaje.transform.localScale = new Vector3(-1, 1, 1);
        }

        if (_caracola != null && !Input.GetKey(KeyCode.Z))
        {
            soltarCaracola();
        }

        if (_direccionX != 0 && _enSuelo)
        {
            _anim.SetFloat("velX", 1);
        } else {
            _anim.SetFloat("velX", 0);
        }

        if (_enSuelo)
        {
            _anim.SetBool("isGround", true);
        } else {
            _anim.SetBool("isGround", false);
        }

        if(_rb.velocity.y < 0) {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (3f - 1) * Time.deltaTime;
        } else if (_rb.velocity.y > 0 && !Input.GetButton("Jump")) {
            _rb.velocity += Vector2.up * Physics2D.gravity.y * (4.5f - 1) * Time.deltaTime;
        }
    
        if (Input.GetButtonDown("Jump") && _enSuelo) {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.velocity += _direccion * _fuerzaSalto;
        }
    }
}
