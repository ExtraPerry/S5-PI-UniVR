using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GlyphsList 
{
    Giza,               // https://stargate.fandom.com/wiki/Point_of_origin
    Crater,             // http://en.wikipedia.org/wiki/Crater_(constellation)
    Virgo,              // http://en.wikipedia.org/wiki/Virgo_(constellation)
    Boötes,             // http://en.wikipedia.org/wiki/Bo%C3%B6tes
    Centaurus,          // http://en.wikipedia.org/wiki/Centaurus
    Libra,              // http://en.wikipedia.org/wiki/Libra_(constellation)
    Serpens_Caput,      // http://en.wikipedia.org/wiki/Serpens
    Norma,              // http://en.wikipedia.org/wiki/Norma_(constellation)
    Scorpius,           // http://en.wikipedia.org/wiki/Scorpius
    Corona_Australis,   // http://en.wikipedia.org/wiki/Corona_Australis
    Scutum,             // https://en.wikipedia.org/wiki/Scutum_(constellation)
    Sagittarius,        // http://en.wikipedia.org/wiki/Sagittarius_(constellation)
    Aquila,             // http://en.wikipedia.org/wiki/Aquila_(constellation)
    Microsopium,        // http://en.wikipedia.org/wiki/Microscopium
    Capricornus,        // http://en.wikipedia.org/wiki/Capricornus
    Piscis_Austrinus,   // http://en.wikipedia.org/wiki/Piscis_Austrinus
    Equuleus,           // http://en.wikipedia.org/wiki/Equuleus
    Aquarius,           // http://en.wikipedia.org/wiki/Aquarius_(constellation)
    Pegasus,            // http://en.wikipedia.org/wiki/Pegasus_(constellation)
    Sculptor,           // http://en.wikipedia.org/wiki/Sculptor_(constellation)
    Pisces,             // http://en.wikipedia.org/wiki/Pisces_(constellation)
    Andromeda,          // http://en.wikipedia.org/wiki/Andromeda_(constellation)
    Triangulum,         // http://en.wikipedia.org/wiki/Triangulum
    Aries,              // http://en.wikipedia.org/wiki/Aries_(constellation)
    Perseus,            // http://en.wikipedia.org/wiki/Perseus_(constellation)
    Cetus,              // http://en.wikipedia.org/wiki/Cetus
    Taurus,             // http://en.wikipedia.org/wiki/Taurus_(constellation)
    Auriga,             // http://en.wikipedia.org/wiki/Auriga_(constellation)
    Eridanus,           // http://en.wikipedia.org/wiki/Eridanus_(constellation)
    Orion,              // http://en.wikipedia.org/wiki/Orion_(constellation)
    Canis_Minor,        // http://en.wikipedia.org/wiki/Canis_Minor
    Monoceros,          // http://en.wikipedia.org/wiki/Monoceros
    Gemini,             // http://en.wikipedia.org/wiki/Gemini_(constellation)
    Hydra,              // http://en.wikipedia.org/wiki/Hydra_(constellation)
    Lynx,               // http://en.wikipedia.org/wiki/Lynx_(constellation)
    Cancer,             // http://en.wikipedia.org/wiki/Cancer_(constellation)
    Sextans,            // http://en.wikipedia.org/wiki/Sextans
    Leo_Minor,          // http://en.wikipedia.org/wiki/Leo_Minor
    Leo                 // http://en.wikipedia.org/wiki/Leo_(constellation)

}



public class Glyphs : MonoBehaviour
{
    [SerializeField]
    private Texture[] glyphTextures = new Texture[39];
}
