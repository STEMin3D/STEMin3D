//This code partly comes from user bdougie on Github, but the constellation map code is hand written
//other portions were also added to meet specific needs of this project
using System.Collections.Generic;
using UnityEngine;

public class StarField : MonoBehaviour
{
    [Range(0, 100)]
    [SerializeField] private float starSizeMin = 0f;
    [Range(0, 100)]
    [SerializeField] private float starSizeMax = 5f;
    private List<StarDataLoader.Star> stars;
    private List<GameObject> starObjects;
    private Dictionary<int, GameObject> constellationVisible = new();
    private readonly int starFieldScale = 400;
    private ConstellationFinder cfinder;
    private GameObject[] labelArray;
    public GameObject mainCamera;
    private Vector3 camDirection;
    public bool constellationsShouldDissapear;
    void Start()
    {
        // Read in the star data.
        StarDataLoader sdl = new();
        stars = sdl.LoadData();
        starObjects = new();
        foreach (StarDataLoader.Star star in stars)
        {
            // Create star game objects.
            GameObject stargo = GameObject.CreatePrimitive(PrimitiveType.Quad);
            stargo.transform.parent = transform;
            stargo.name = $"HR {star.catalog_number}";
            stargo.transform.localPosition = star.position * starFieldScale;
            //stargo.transform.localScale = Vector3.one * Mathf.Lerp(starSizeMin, starSizeMax, star.size);
            stargo.transform.LookAt(transform.position);
            stargo.transform.Rotate(0, 180, 0);
            Material material = stargo.GetComponent<MeshRenderer>().material;
            material.shader = Shader.Find("Unlit/StarShader");
            material.SetFloat("_Size", Mathf.Lerp(starSizeMin, starSizeMax, star.size));
            material.color = star.colour;
            starObjects.Add(stargo);
        }
        cfinder = mainCamera.GetComponent<ConstellationFinder>();
        labelArray = cfinder.GetLabelArray();
        for (int i = 0; i < labelArray.Length; i++)
        {
            ToggleConstellation(i);
        }
    }

    // Could also do in Update with Time.deltatime scaling.
    private void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            Camera.main.transform.RotateAround(Camera.main.transform.position, Camera.main.transform.right, Input.GetAxis("Mouse Y"));
            Camera.main.transform.RotateAround(Camera.main.transform.position, Vector3.up, -Input.GetAxis("Mouse X"));
        }
        return;
    }

    private void Update()
    {
        if (constellationsShouldDissapear)
        {
            camDirection = mainCamera.transform.forward * cfinder.distance;
            for (int i = 0; i < labelArray.Length; i++)
            {
                if ((camDirection - labelArray[i].transform.position).magnitude < 200 && !constellationVisible.ContainsKey(i))
                {
                    ToggleConstellation(i);
                }
                else if ((camDirection - labelArray[i].transform.position).magnitude >= 200 && constellationVisible.ContainsKey(i))
                {
                    ToggleConstellation(i);
                }
            }
        }
    }

    private void OnValidate()
    {
        if (starObjects != null)
        {
            for (int i = 0; i < starObjects.Count; i++)
            {
                // Update the size set in the shader.
                Material material = starObjects[i].GetComponent<MeshRenderer>().material;
                material.SetFloat("_Size", Mathf.Lerp(starSizeMin, starSizeMax, stars[i].size));
            }
        }
    }

    // A constellation is a tuple of the stars and the lines that join them.
    private readonly List<(int[], int[])> constellations = new() {
    // Andromeda
    (new int[] { 15, 337, 603, 165, 464, 8762, 8961, 269, 215, 458, 8976, 335, 8965, 154, 3546, 5516, 68, 226, 271, 163},
     new int[] { 15,165, 165,337, 337,603, 337,269, 269,226, 226,335, 337,154,
                 154,68, 68,8965, 8965,8976, 8976,8961, 8965,8762, 154,165, 
                 165,163, 163,215, 215,271}),
    // Antlia
    (new int[] { 4104, 3765, 4273},
     new int[] { 4104,3765, 4104,4273}),
    // Apus
    (new int[] { 5470, 6102, 6163, 6020},
     new int[] { 5470,6020, 6020,6163, 6020,6102, 6102,6163}),
    // Aquarius
    (new int[] { 7950, 7990, 8232, 8418, 8414, 8539, 8559, 8597, 8518, 8499, 8512, 8698, 8850, 8834, 8841, 8709, 8679, 8812, 8892},
     new int[] { 7950,7990, 7990,8232, 8232,8418, 8232,8414, 8414,8539, 8539,8559,
                 8559,8597, 8559,8518, 8518,8414, 8414,8499, 8499,8512, 8512,8698,
                 8698,8834, 8834,8850, 8850,8841, 8841,8709, 8709,8679, 8679,8698,
                 8841,8812, 8841,8892}),
    // Aquila
    (new int[] { 7193, 7236, 7235, 7176, 7377, 7447, 7710, 7510, 7525, 7557, 7602},
     new int[] { 7193,7236, 7236,7235, 7235,7176, 7235,7377, 7236,7377, 7236,7447, 7447,7710,
                 7710,7570, 7570,7377, 7377,7525, 7525,7557, 7557,7602}),
    // Ara
    (new int[] { 6743, 6510, 6461, 6462, 6500, 6229, 6285, 6295, 6468},
     new int[] { 6743,6510, 6510,6461, 6461,6462, 6462,6500, 6500,6229, 6229,6285, 6285,6295, 6295,6468, 6468,6510}),
    // Aries
    (new int[] { 546, 553, 617, 838},
     new int[] { 546,553, 553,617, 617,838}),
    // Auriga
    (new int[] { 1612, 1605, 1708, 2088, 2077, 2095, 1791, 1577, 1641},
     new int[] { 1612,1605, 1605,1708, 1708,2088, 2088,2077, 2077,1708, 2088,2095, 2095,1791, 1791,1577, 1577,1641, 1641,1708}),
    // Bootes
    (new int[] { 5185, 5235, 5340, 5478, 5429, 5435, 5602, 5681, 5505, 5351, 5329, 5404},
     new int[] { 5185,5235, 5235,5340, 5340,5478, 5340,5429, 5429,5435, 5435,5602, 5602,5681, 5681,5505,
                 5505,5340, 5435,5351, 5351,5329, 5329,5404, 5404,5351}),
    // Caelum
    (new int[] { 1443, 1502, 1503, 1652},
     new int[] { 1443,1502, 1502,1503, 1503,1652}),
    // Camelopardalis
    (new int[] { 1568, 1603, 1542, 1155, 1035, 2209, 2527},
     new int[] { 1568,1603, 1603,1542, 1542,1148, 1148,1155, 1155,1035, 1542,2209, 2209,2527}),
    // Cancer, courtesy of bdougie on Github
    (new int[] { 3475, 3449, 3461, 3572, 3249},
     new int[] { 3475,3449, 3449,3461, 3461,3572, 3461,3249}),
    // Canes Venatici
    (new int[] { 4914, 4915, 4785},
     new int[] { 4914,4915, 4915,4785}),
    // Canis Major
    (new int[] { 2596, 2657, 2574, 2491, 2294, 2429, 2580, 2618, 2646, 2693, 2749, 2827},
     new int[] { 2596,2657, 2657,2574, 2574,2596, 2596,2491, 2491,2294, 2294,2429, 2429,2580, 2580,2618,
                 2618,2646, 2646,2693, 2693,2491, 2693,2749, 2749,2827}),
    // Canis Minor
    (new int[] { 2943, 2845},
     new int[] { 2943,2845}),
    // Capricornus
    (new int[] { 7754, 7776, 7830, 7936, 7980, 8204, 8260, 8322, 8278, 8167, 8075},
     new int[] { 7754,7776, 7776,7830, 7830,7936, 7936,7980, 7980,8204, 8204,8260, 8260,8322, 8322,8278, 8278,8167, 8167,8075, 8075,7754}),
    // Carina, I am including the connectors to Vela in this constellation
    (new int[] { 2326, 3685, 4037, 4199, 4050, 4140, 3699, 3307, 3117, 4325, 4352, 4337, 4257, 3485, 3207},
     new int[] { 2326,3685, 3685,4037, 4037,4199, 4199,4140, 4140,4050, 4050,3699, 3699,3307, 3307,3117,  
                 4199,4325, 4325,4352, 4352,4337, 4337,4257, 4257,4140, 3699,3485, 3117,3207}),
    // Cassiopeia
    (new int[] { 542, 403, 264, 168, 21},
     new int[] { 542,403, 403,264, 264,168, 168,21}),
    // Centaurus
    (new int[] { 4390, 4621, 4743, 4441, 4638, 4819, 5132, 5267, 5459, 5231, 5193, 5190, 5288, 5367, 5285,
                 5248, 5440, 5576, 5249, 5089, 5028, 4874},
     new int[] { 4390,4621, 4621,4743, 4441,4638, 4638,4743, 4743,4819, 4819,5132, 5132,5267, 5267,5459,
                 5231,5132, 5231,4819, 5231,5193, 5193,5190, 5190,5288, 5288,5367, 5367,5285, 5285,5248,
                 5248,5440, 5440,5576, 5248,5249, 5249,5231, 5190,5089, 5089,5028, 5028,4874}),
    // Cepheus
    (new int[] { 8974, 8238, 8694, 8162, 8316, 8494, 8465, 8571, 7957, 7850},
     new int[] { 8974,8238, 8974,8694, 8238,8694, 8238,8162, 8162,8316, 8316,8494, 8494,8465, 8465,8571, 8571,8694,
                 8162,7957, 7957,7850}),
    // Cetus
    (new int[] { 804, 754, 718, 813, 896, 911, 779, 681, 539, 402, 334, 74, 188, 509},
     new int[] { 804,754, 754,718, 718,813, 813,896, 896,911, 911,804, 804,779, 779,681, 681,539, 539,402,
                 402,334, 334,74, 74,188, 188,509, 509,539}),
    // Chamaeleon
    (new int[] { 3340, 3318, 4174, 4583, 4674, 4234, 4206},
     new int[] { 3340,3318, 3318,4174, 4174,4583, 4583,4674, 4674,4234, 4234,4206, 4206,4174}),
    // Circinus
    (new int[] { 5670, 5463, 5704},
     new int[] { 5670,5463, 5463,5704}),
    // Columba
    (new int[] { 1862, 1956, 2040, 2296, 2120},
     new int[] { 1862,1956, 1956,2040, 2040,2296, 2040,2120}),
    // Coma Berenices
    (new int[] { 4968, 4983, 4737},
     new int[] { 4968,4983, 4983,4737}),
    // Corona Australis
    (new int[] { 7226, 7254, 7259, 7242, 6951},
     new int[] { 7226,7254, 7254,7259, 7259,7242, 7242,6951}),
    // Corona Borealis
    (new int[] { 5971, 5947, 5889, 5849, 5793, 5747, 5778},
     new int[] { 5971,5947, 5947,5889, 5889,5849, 5849,5793, 5793,5747, 5747,5778}),
    // Corvus
    (new int[] { 4623, 4630, 4662, 4757, 4786, 4775},
     new int[] { 4623,4630, 4630,4662, 4662,4757, 4757,4786, 4786,4630, 4757,4775}),
    // Crater
    (new int[] { 4287, 4343, 4405, 4382, 4514, 4567, 4402, 4468},
     new int[] { 4287,4343, 4343,4405, 4405,4382, 4382,4287, 4405,4514, 4514,4567, 4382,4402, 4402,4468}),
    // Crux
    (new int[] { 4853, 4656, 4730, 4763},
     new int[] { 4853,4656, 4730,4763}),
    // Cygnus
    (new int[] { 7417, 7615, 7796, 7924, 8028, 8115, 7949, 7528, 7408, 7751, 7328},
     new int[] { 7417,7615, 7615,7796, 7796,7924, 7924,8028, 8028,8115, 8115,7949,
                 7949,7796, 7796,7528, 7528,7408, 7408,7751, 7751,7924, 7408,7328}),
    // Delphinus
    (new int[] { 7852, 7882, 7906, 7948, 7928},
     new int[] { 7852,7882, 7882,7906, 7906,7948, 7948,7928, 7928,7882}),
    // Dorado
    (new int[] { 1338, 1465, 1922, 1674, 2015, 2102},
     new int[] { 1338,1465, 1465,1922, 1922,1674, 1674,1465, 1922,2015, 2015,2102, 2102,1922}),
    // Draco
    (new int[] { 6688, 6555, 6536, 6705, 7310, 7582, 6920, 6927, 6596, 6396, 6132, 5986, 5744, 5291, 4787, 4434},
     new int[] { 6688,6555, 6555,6536, 6536,6705, 6705,6688, 6688,7310, 7310,7582, 7310,6920, 6920,6927,
                 6920,6596, 6596,6396, 6396,6132, 6132,5986, 5986,5744, 5744,5291, 5291,4787, 4787,4434}),
    // Equuleus
    (new int[] { 8131, 8123, 8097},
     new int[] { 8131,8123, 8123,8097}),
    // Eridanus
    (new int[] { 1666, 1520, 1463, 1298, 1231, 1162, 1136, 1084, 874, 811, 818, 850, 919, 1003, 1088,
                 1173, 1181, 1213, 1240, 1453, 1464, 1393, 1347, 1195, 1106, 1008, 897, 794, 789, 721, 566, 472},
     new int[] { 1666,1520, 1520,1463, 1463,1298, 1298,1231, 1231,1162, 1162,1136, 1136,1084, 1084,874, 874,811, 811,818,
                 818,850, 850,919, 919,1003, 1003,1088, 1088,1173, 1173,1181, 1181,1213, 1213,1240, 1240,1453, 1453,1464,
                 1464,1393, 1393,1347, 1347,1195, 1195,1106, 1106,1008, 1008,897, 897,794, 794,789, 789,721, 721,566, 566,472}),
    // Fornax
    (new int[] { 963, 841, 612},
     new int[] { 963,841, 841,612}),
     // Gemini, courtesy of bdougie on Github
    (new int[] { 2890, 2891, 2990, 2421, 2777, 2473, 2650, 2216, 2895,
                 2343, 2484, 2286, 2134, 2763, 2697, 2540, 2821, 2905, 2985},
     new int[] { 2890,2697, 2990,2905, 2697,2473, 2905,2777, 2777,2650,
                 2650,2421, 2473,2286, 2286,2216, 2473,2343, 2216,2134,
                 2763,2484, 2763,2777, 2697,2540, 2697,2821, 2821,2905, 2905,2985}),
    // Grus
    (new int[] { 8353, 8411, 8486, 8556, 8636, 8675, 8747, 8425},
     new int[] { 8353,8411, 8411,8486, 8486,8556, 8556,8636, 8636,8675, 8675,8747, 8425,8636, 8425,8556}),
    // Hercules
    (new int[] { 5914, 6023, 6092, 6168, 6220, 6588, 6695, 6484, 6418, 6324, 6212, 6410, 6406, 6148,
                 6095, 6117, 6159, 6526, 6623, 6703, 6779},
     new int[] { 5914,6023, 6023,6092, 6092,6168, 6168,6220, 6588,6695, 6695,6484, 6484,6418, 6418, 6220,
                 6418,6324, 6324,6212, 6212,6220, 6324,6410, 6410,6406, 6406,6148, 6148,6212, 6148,6095,
                 6095,6117, 6117,6159, 6410,6526, 6526,6623, 6623,6703, 6703,6779}),
    // Horologium
    (new int[] { 1326, 810, 778, 802, 934, 909},
     new int[] { 1326,810, 810,778, 778,802, 802,934, 934,909}),
    // Hydra
    (new int[] { 3492, 3454, 3418, 3410, 3482, 3665, 3845, 3748, 3903, 3994, 4094, 4232, 4287, 4343, 4450, 4552, 5020, 5287, 5526},
     new int[] { 3492,3454, 3454,3418, 3418,3410, 3410,3482, 3482,3492, 3492,3665, 3665,3845, 3845,3748, 3748,3903,
                 3903,3994, 3994,4094, 4094,4232, 4232,4287, 4287,4343, 4343,4450, 4450,4552, 4552,5020, 5020,5287, 5287,5526}),
    // Hydrus
    (new int[] { 98, 591, 1208},
     new int[] { 98,591, 591,1208, 1208,98}),
    // Indus
    (new int[] { 7869, 8140, 8368, 7986, 7920},
     new int[] { 7869,8140, 8140,8368, 8368,7986, 7986,7920, 7920,7869}),
    // Lacerta
    (new int[] { 8572, 8585, 8538, 8541, 8523, 8579, 8632, 8485, 8498},
     new int[] { 8572,8585, 8585,8538, 8538,8541, 8541,8572, 8572,8523, 8523,8579, 8579,8632, 8632,8572, 8572,8485, 8485,8498}),
    // Leo, courtesy of bdougie on Github
    (new int[] { 3982, 4534, 4057, 4357, 3873, 4031, 4359, 3975, 4399, 4386, 3905, 3773, 3731},
     new int[] { 4534,4357, 4534,4359, 4357,4359, 4357,4057, 4057,4031,
                 4057,3975, 3975,3982, 3975,4359, 4359,4399, 4399,4386,
                 4031,3905, 3905,3873, 3873,3975, 3873,3773, 3773,3731, 3731,3905}),
    // Leo Minor, courtesy of bdougie on Github
    (new int[] { 3800, 3974, 4100, 4247, 4090},
     new int[] { 3800,3974, 3974,4100, 4100,4247, 4247,4090, 4090,3974}),
    // Lepus
    (new int[] { 1865, 1998, 2085, 2155, 2035, 1983, 1829, 1654, 1702, 1705, 1756},
     new int[] { 1865,1998, 1998,2085, 2085,2155, 2155,2035, 2035,1983, 1983,1829, 1829,1865, 1829,1654,
                 1654,1702, 1702,1865, 1702,1705, 1702,1756}),
    // Libra
    (new int[] { 5812, 5794, 5787, 5685, 5531, 5603},
     new int[] { 5812,5794, 5794,5787, 5787,5685, 5685,5531, 5531,5787, 5531,5603}),
    // Lupus
    (new int[] { 5469, 5649, 5683, 5708, 5776, 5948, 5883, 5705, 5695, 5571},
     new int[] { 5469,5649, 5649,5683, 5683,5708, 5708,5776, 5776,5948, 5948,5649, 5948,5883,
                 5883,5705, 5705,5948, 5776,5695, 5695,5571}),
    // Lynx, courtesy of bdougie on Github
    (new int[] { 3705, 3690, 3612, 3579, 3275, 2818, 2560, 2238},
     new int[] { 3705,3690, 3690,3612, 3612,3579, 3579,3275, 3275,2818,
                 2818,2560, 2560,2238}),
    // Lyra
    (new int[] { 7001, 7051, 7056, 7139, 7178, 7106},
     new int[] { 7001,7051, 7051,7056, 7056,7001, 7056,7139, 7139,7178, 7178,7106, 7106,7056}),
    // Mensa - IAU officially does not designate a shape to Mensa, so I gave it the modern interpretation, alpha-gamma-eta-mu
    (new int[] { 2261, 1953, 1629, 1541},
     new int[] { 2261,1953, 1953,1629, 1629,1541}),
    // Microscopium - IAU officially does not designate a shape to Mensa, so I gave it the modern interpretation, alpha-gamma-epsilon-theta1-iota-alphau
    (new int[] { 7965, 8039, 8135, 8151, 7943},
     new int[] { 7965,8039, 8039,8135, 8135,8151, 8151,7943, 7943,7965}),
    // Monceros, courtesy of bdougie on Github
    (new int[] { 2970, 3188, 2714, 2356, 2227, 2506, 2298, 2385, 2456, 2479},
     new int[] { 2970,3188, 3188,2714, 2714,2356, 2356,2227, 2714,2506,
                 2506,2298, 2298,2385, 2385,2456, 2479,2506, 2479,2385}),
    // Musca
    (new int[] { 4520, 4671, 4798, 4844, 4923, 4773},
     new int[] { 4520,4671, 4671,4798, 4798,4844, 4844,4923, 4923,4773, 4773,4798}),
    // Norma
    (new int[] { 6072, 5962, 5980, 6115},
     new int[] { 6072,5962, 5962,5980, 5980,6115, 6115,6072}),
    // Octans
    (new int[] { 8254, 8630, 5339},
     new int[] { 8254,8630, 8630,5339, 5339,8254}),
    // Ophiuchus
    (new int[] { 6698, 6629, 6603, 6556, 6299, 6056, 6075, 6129, 6175, 6378, 6147, 6104, 6113, 6453, 6492},
     new int[] { 6698,6629, 6629,6603, 6603,6556, 6556,6299, 6299,6056, 6056,6075, 6075,6129,
                 6129,6175, 6175,6378, 6299,6175, 6175,6147, 6147,6104, 6104,6113, 6378,6453, 6453,6492}),
    // Orion, courtesy of bdougie on Github
    (new int[] { 1948, 1903, 1852, 2004, 1713, 2061, 1790, 1907, 2124,
                 2199, 2135, 2047, 2159, 1543, 1544, 1570, 1552, 1567},
     new int[] { 1713,2004, 1713,1852, 1852,1790, 1852,1903, 1903,1948,
                 1948,2061, 1948,2004, 1790,1907, 1907,2061, 2061,2124,
                 2124,2199, 2199,2135, 2199,2159, 2159,2047, 1790,1543,
                 1543,1544, 1544,1570, 1543,1552, 1552,1567, 2135,2047}),
    // Pavo
    (new int[] { 6582, 6745, 6855, 7074, 7665, 7107, 7790, 8181, 7612, 7590, 6982},
     new int[] { 6582,6745, 6745,6855, 6855,7074, 7074,7665, 7665,7107, 7107,6745, 7665,7790, 7790,8181, 8181,7665,
                 7665,7612, 7612,7590, 7612,6982}),
    // Pegasus
    (new int[] { 15, 8775, 8781, 39, 8634, 8450, 8308, 8650, 8454, 8667, 8430, 8315},
     new int[] { 15,8775, 8775,8781, 8781,39, 39,15, 8781,8634, 8634,8450, 8450,8308, 8775,8650, 8650,8454,
                 8775,8667, 8667,8430, 8430,8315}),
    // Perseus
    (new int[] { 1131, 1203, 1228, 1220, 1087, 1017, 937, 941, 936, 921, 915, 834, 854},
     new int[] { 1131,1203, 1203,1228, 1228,1220, 1220,1087, 1087,1017, 1017,937, 937,941, 941,936, 936,1220, 936, 921,
                 1017,915, 915,834, 834,854, 854,915, 854,937}),
    // Phoenix
    (new int[] { 25, 99, 322, 429, 440, 338},
     new int[] { 25,99, 99,322, 322,429, 429,440, 440,338, 338,322}),
    // Pictor
    (new int[] { 2550, 2042, 2020},
     new int[] { 2550,2042, 2042,2020}),
    // Pisces
    (new int[] { 352, 383, 360, 437, 510, 596, 489, 434, 294, 224, 9072, 8969, 8916, 8852, 8911, 8984},
     new int[] { 352,383, 383,360, 360,352, 360,437, 437,510, 510,596, 596,489, 489,434, 434,294, 294,224,
                 224,9072, 9072,8969, 8969,8916, 8916,8852, 8852,8911, 8911,8984, 8984,8969}),
    // Piscis Austrinus
    (new int[] { 8728, 8628, 8431, 8326, 8305, 8576, 8695, 8720},
     new int[] { 8728,8628, 8628,8431, 8431,8326, 8326,8305, 8305,8431, 8431,8576, 8576,8695, 8695,8720, 8720,8728}),
    // Puppis, including connections to Vela and Carina here
    (new int[] { 3207, 3165, 3185, 3045, 3034, 2996, 2922, 2948, 2773, 2451, 2326},
     new int[] { 3207,3165, 3165,3185, 3185,3045, 3045,3034, 3034,2996, 2996,2922, 2922,2948, 2948,3045,
                 2922,2773, 2773,2451, 2451,2326}),
    // Pyxis
    (new int[] { 3518, 3468, 3438, 3165},
     new int[] { 3518,3468, 3468,3438, 3438,3165}),
    // Reticulum
    (new int[] { 1336, 1355, 1266, 1247, 1175},
     new int[] { 1336,1355, 1355,1266, 1266,1247, 1247,1175, 1175,1336}),
    // Sagitta
    (new int[] { 7635, 7536, 7479, 7488},
     new int[] { 7635,7536, 7536,7479, 7536,7488}),
    // Sagittarius
    (new int[] { 7340, 7264, 7217, 7150, 6812, 6913, 7039, 6859, 6746, 6879, 6832, 7194, 7234, 7121},
     new int[] { 7340,7264, 7264,7217, 7217,7150, 7150,7264, 6812,6913, 6913,7039, 7039,6859, 6859,6812,
                 6859,6746, 6746,6879, 6879,6859, 6879,6832, 6879,7194, 7194,7039, 7194,7234, 7234,7121, 7121,7039}),
    // Scorpius
    (new int[] { 6630, 6527, 6508, 6580, 6615, 6553, 6380, 6271, 6247, 6241, 6165, 6134, 6084, 5953, 5984, 5944, 5928},
     new int[] { 6630,6527, 6527,6508, 6508,6580, 6580,6615, 6615,6553, 6553,6380, 6380,6271, 6271,6247, 6247,6241, 6241,6165,
                 6165,6134, 6134,6084, 6084,5953, 5953,5984, 5953,5944, 5944,5928}),
    // Sculptor
    (new int[] { 280, 84, 9016, 8863, 8937},
     new int[] { 280,84, 84,9016, 9016,8863, 8863,8937}),
    // Scutum
    (new int[] { 6973, 7063, 7032, 7020, 6930},
     new int[] { 6973,7063, 7063,7032, 7032,7020, 7020,6930, 6930,6973}),
    // Serpens
    (new int[] { 5881, 5888, 5892, 5854, 5788, 5867, 5933, 5879, 5842},
     new int[] { 5881,5888, 5888,5892, 5892,5854, 5854,5788, 5788,5867, 5867,5933, 5933,5879, 5879,5842, 5842,5867}),
    // Sextans
    (new int[] { 4116, 4119, 3981, 3909},
     new int[] { 4116,4119, 4119,3981, 3981,3909}),
    // Taurus
    (new int[] { 1791, 1409, 1373, 1346, 1412, 1457, 1910,1239, 1038, 1251, 1030, 1101},
     new int[] { 1791,1409, 1409,1373, 1373,1346, 1346,1412, 1412,1457, 1457,1910, 1346,1239, 1239,1038, 1038,1251, 1030,1101}),
    // Telescopium
    (new int[] { 6905, 6897, 6783},
     new int[] { 6905,6897, 6897,6783}),
    // Triangulum
    (new int[] { 544, 622, 664},
     new int[] { 544,622, 622,664, 664,544}),
    // Triangulum Australe
    (new int[] { 6217, 5897, 5771, 5671},
     new int[] { 6217,5897, 5897,5771, 5771,5671, 5671,6217}),
    // Tucana
    (new int[] { 8502, 8848, 126, 77, 9076, 8540},
     new int[] { 8502,8848, 8848,126, 126,77, 77,9076, 9076,8540, 8540,8502}),
    // Ursa Major, courtesy of bdougie on Github
    (new int[] { 3569, 3594, 3775, 3888, 3323, 3757, 4301, 4295, 4554, 4660,
                 4905, 5054, 5191, 4518, 4335, 4069, 4033, 4377, 4375},
     new int[] { 3569,3594, 3594,3775, 3775,3888, 3888,3323, 3323,3757,
                 3757,3888, 3757,4301, 4301,4295, 4295,3888, 4295,4554,
                 4554,4660, 4660,4301, 4660,4905, 4905,5054, 5054,5191,
                 4554,4518, 4518,4335, 4335,4069, 4069,4033, 4518,4377, 4377,4375}),
    // Ursa Minor
    (new int[] { 424, 6789, 6322, 5903, 5826, 5563, 5735, 6116},
     new int[] { 424,6789, 6789,6322, 6322,5903, 5903,5826, 5826,5563, 5563,5735, 5735,6116, 6116,5903}),
    // Vela
    (new int[] { 3207, 3485, 3734, 3940, 4216, 4023, 3786, 3634},
     new int[] { 3207,3485, 3485,3734, 3734,3940, 3940,4216, 4216,4023, 4023,3786, 3786,3634, 3634,3207}),
    // Virgo
    (new int[] { 5487, 5338, 5107, 5264, 5511, 4826, 4910, 4932, 4963, 5056, 4689, 4540, 4517, 4608},
     new int[] { 5487,5338, 5338,5107, 5107,5264, 5264,5511, 5107,4826, 4826,4910, 4910,4932,
                 4826,4963, 4963,5056, 4826,4689, 4689,4540, 4540,4517, 4517,4608, 4608,4689}),
    // Volans
    (new int[] { 3615, 3347, 3223, 2803, 2736},
     new int[] { 3615,3347, 3347,3223, 3223,3615, 3223,2803, 2803,2736, 2736,3223}),
    // Vulpecula
    (new int[] { 7405, 7653},
     new int[] { 7405,7653}),
  };

    void ToggleConstellation(int index)
    {
        // Safety check the index is valid.
        if ((index < 0) || (index >= constellations.Count))
        {
            return;
        }

        // Toggle on or off.
        if (constellationVisible.ContainsKey(index))
        {
            RemoveConstellation(index);
        }
        else
        {
            CreateConstellation(index);
        }
    }

    void CreateConstellation(int index)
    {
        int[] constellation = constellations[index].Item1;
        int[] lines = constellations[index].Item2;

        // Change the colours of the stars
        foreach (int catalogNumber in constellation)
        {
            // Remember list is 0-up catalog numbers are 1-up.
            starObjects[catalogNumber - 1].GetComponent<MeshRenderer>().material.color = Color.white;
        }

        GameObject constellationHolder = new($"Constellation {index}");
        constellationHolder.transform.parent = transform;
        constellationVisible[index] = constellationHolder;

        // Draw the constellation lines.
        for (int i = 0; i < lines.Length; i += 2)
        {
            // Parent it to our constellation object so we can delete them all later.
            GameObject line = new("Line");
            line.transform.parent = constellationHolder.transform;
            // Defaults to white and width 1 which works for us.
            LineRenderer lineRenderer = line.AddComponent<LineRenderer>();
            // Doesn't get assigned a material so just dig out one that works.
            lineRenderer.material = new Material(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            // Disable useWorldSpace so it will track the parent game object.
            lineRenderer.useWorldSpace = false;
            Vector3 pos1 = starObjects[lines[i] - 1].transform.position;
            Vector3 pos2 = starObjects[lines[i + 1] - 1].transform.position;
            // Offset them so they don't occlude the stars, 3 chosen by trial and error.
            Vector3 dir = (pos2 - pos1).normalized * 3;
            lineRenderer.positionCount = 2;
            lineRenderer.SetPosition(0, pos1 + dir);
            lineRenderer.SetPosition(1, pos2 - dir);
        }
    }

    void RemoveConstellation(int index)
    {
        int[] constallation = constellations[index].Item1;

        // Toggling off set the stars back to the original colour.
        foreach (int catalogNumber in constallation)
        {
            // Remember list is 0-up catalog numbers are 1-up.
            starObjects[catalogNumber - 1].GetComponent<MeshRenderer>().material.color = stars[catalogNumber - 1].colour;
        }
        // Remove the constellation lines.
        Destroy(constellationVisible[index]);
        // Remove from our dictionary as it's now off.
        constellationVisible.Remove(index);
    }

}
