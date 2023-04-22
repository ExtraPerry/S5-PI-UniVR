using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Glyph 
{
    Giza,               // 1 https://stargate.fandom.com/wiki/Point_of_origin
    Crater,             // 2 http://en.wikipedia.org/wiki/Crater_(constellation)
    Virgo,              // 3 http://en.wikipedia.org/wiki/Virgo_(constellation)
    Boötes,             // 4 http://en.wikipedia.org/wiki/Bo%C3%B6tes
    Centaurus,          // 5 http://en.wikipedia.org/wiki/Centaurus
    Libra,              // 6 http://en.wikipedia.org/wiki/Libra_(constellation)
    Serpens_Caput,      // 7 http://en.wikipedia.org/wiki/Serpens
    Norma,              // 8 http://en.wikipedia.org/wiki/Norma_(constellation)
    Scorpius,           // 9 http://en.wikipedia.org/wiki/Scorpius
    Corona_Australis,   // 10 http://en.wikipedia.org/wiki/Corona_Australis
    Scutum,             // 11 https://en.wikipedia.org/wiki/Scutum_(constellation)
    Sagittarius,        // 12 http://en.wikipedia.org/wiki/Sagittarius_(constellation)
    Aquila,             // 13 http://en.wikipedia.org/wiki/Aquila_(constellation)
    Microsopium,        // 14 http://en.wikipedia.org/wiki/Microscopium
    Capricornus,        // 15 http://en.wikipedia.org/wiki/Capricornus
    Piscis_Austrinus,   // 16 http://en.wikipedia.org/wiki/Piscis_Austrinus
    Equuleus,           // 17 http://en.wikipedia.org/wiki/Equuleus
    Aquarius,           // 18 http://en.wikipedia.org/wiki/Aquarius_(constellation)
    Pegasus,            // 19 http://en.wikipedia.org/wiki/Pegasus_(constellation)
    Sculptor,           // 20 http://en.wikipedia.org/wiki/Sculptor_(constellation)
    Pisces,             // 21 http://en.wikipedia.org/wiki/Pisces_(constellation)
    Andromeda,          // 22 http://en.wikipedia.org/wiki/Andromeda_(constellation)
    Triangulum,         // 23 http://en.wikipedia.org/wiki/Triangulum
    Aries,              // 24 http://en.wikipedia.org/wiki/Aries_(constellation)
    Perseus,            // 25 http://en.wikipedia.org/wiki/Perseus_(constellation)
    Cetus,              // 26 http://en.wikipedia.org/wiki/Cetus
    Taurus,             // 27 http://en.wikipedia.org/wiki/Taurus_(constellation)
    Auriga,             // 28 http://en.wikipedia.org/wiki/Auriga_(constellation)
    Eridanus,           // 29 http://en.wikipedia.org/wiki/Eridanus_(constellation)
    Orion,              // 30 http://en.wikipedia.org/wiki/Orion_(constellation)
    Canis_Minor,        // 31 http://en.wikipedia.org/wiki/Canis_Minor
    Monoceros,          // 32 http://en.wikipedia.org/wiki/Monoceros
    Gemini,             // 33 http://en.wikipedia.org/wiki/Gemini_(constellation)
    Hydra,              // 34 http://en.wikipedia.org/wiki/Hydra_(constellation)
    Lynx,               // 35 http://en.wikipedia.org/wiki/Lynx_(constellation)
    Cancer,             // 36 http://en.wikipedia.org/wiki/Cancer_(constellation)
    Sextans,            // 37 http://en.wikipedia.org/wiki/Sextans
    Leo_Minor,          // 38 http://en.wikipedia.org/wiki/Leo_Minor
    Leo                 // 39 http://en.wikipedia.org/wiki/Leo_(constellation)

}



public class Glyphs : MonoBehaviour
{
    [SerializeField]
    private Texture[] glyphTextures = new Texture[39];
    [SerializeField]
    private Material blankGlyph;
    [SerializeField]
    private Material[] glyphMaterials = new Material[39];

    public Texture GetSymboleTexture(Glyph glyph)
    {
        return glyphTextures[(int)glyph];
    }

    public Material GetGlyphMaterial(Glyph glyph)
    {
        return glyphMaterials[(int)glyph];
    }

    public Material GetBlankGlyphMaterial()
    {
        return blankGlyph;
    }
}
