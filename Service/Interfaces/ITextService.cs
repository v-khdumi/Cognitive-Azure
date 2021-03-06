﻿using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface ITextService
    {
        Task<double> GetScore(string text);

        Task<string> GetKeyPhrases(string text);
    }
}