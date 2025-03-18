﻿using BaralhoDeCartas.Models.ApiResponses;
using BaralhoDeCartas.Models.Interfaces;

namespace BaralhoDeCartas.Factory.Interfaces
{
    public interface ICartaFactory
    {
        List<ICarta> CriarCartas(CartasResponse response);
    }
}
