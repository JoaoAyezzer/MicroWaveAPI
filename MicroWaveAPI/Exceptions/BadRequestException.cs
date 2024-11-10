namespace MicroWaveAPI.Exceptions;

using System;
public class BadRequestException(string message) : Exception(message);