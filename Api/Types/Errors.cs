using FluentValidation.Results;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Api.Types;

public record Error
{
    public Error(string key, string value)
    {
        Key = key;
        Message = value;
    }

    public Error(string codigo, string key, string value)
    {
        Codigo = codigo;
        Key = key;
        Message = value;
    }

    public string? Codigo { get; private set; }
    public string Key { get; private set; }
    public string Message { get; private set; }

    public static implicit operator Error((string, string) valor) => new(valor.Item1, valor.Item2);
    public static implicit operator Error((string, string, string) valor) => new(valor.Item1, valor.Item2, valor.Item3);
    public static implicit operator Errors(Error error) => new() { error };
    public static implicit operator ValidationFailure(Error error) => new(error.Key, error.Message);
    public static implicit operator Error(ValidationFailure error) => new(error.PropertyName, error.ErrorMessage);
    public static implicit operator bool(Error? error) => error is null;
}

public class Errors : IEnumerable<Error>, IEnumerator<Error>, IEnumerator, IEnumerable, IDisposable
{
    private readonly HashSet<Error> _erros = new();

    public bool IsValid() => !_erros.Any();

    public int Count => _erros.Count;

    public void Add(string key, string value) => _erros.Add((key, value));

    public void Add((string, string) valor) => _erros.Add(valor);

    public void Add(Error erro) => _erros.Add(erro);

    public IEnumerator<Error> GetEnumerator() => _erros.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => _erros.GetEnumerator();

    public bool MoveNext() => GetEnumerator().MoveNext();

    public void Reset() => GetEnumerator().Reset();

    public void Dispose()
    {
        GetEnumerator().Dispose();
        GC.SuppressFinalize(this);
    }

    public Error Current => GetEnumerator().Current;

    object IEnumerator.Current => GetEnumerator().Current;

    public static implicit operator Errors(ValidationResult validationFailure)
    {
        var errors = new Errors();
        validationFailure.Errors.ForEach(failure => errors.Add(failure));
        return errors;
    }

    public static implicit operator bool(Errors errors) => errors is null || !errors._erros.Any();
}