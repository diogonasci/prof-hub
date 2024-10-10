using System.Text.Json.Serialization;

namespace Prof.Hub.Application.Results
{
    public class Result<T> : IResult
    {
        protected Result() { }

        public Result(T value) => Value = value;

        protected internal Result(T value, string successMessage) : this(value) => SuccessMessage = successMessage;

        protected Result(ResultStatus status) => Status = status;

        public static implicit operator T(Result<T> result) => result.Value;
        public static implicit operator Result<T>(T value) => new Result<T>(value);

        public static implicit operator Result<T>(Result result) => new(default(T))
        {
            Status = result.Status,
            Errors = result.Errors,
            SuccessMessage = result.SuccessMessage,
            CorrelationId = result.CorrelationId,
            ValidationErrors = result.ValidationErrors,
        };

        [JsonInclude]
        public T Value { get; init; }

        [JsonIgnore]
        public Type ValueType => typeof(T);
        [JsonInclude]
        public ResultStatus Status { get; protected set; } = ResultStatus.Ok;

        public bool IsSuccess => Status is ResultStatus.Ok or ResultStatus.NoContent or ResultStatus.Created;

        [JsonInclude]
        public string SuccessMessage { get; protected set; } = string.Empty;
        [JsonInclude]
        public string CorrelationId { get; protected set; } = string.Empty;
        [JsonInclude]
        public string Location { get; protected set; } = string.Empty;
        [JsonInclude]
        public IEnumerable<string> Errors { get; protected set; } = [];
        [JsonInclude]
        public IEnumerable<ValidationError> ValidationErrors { get; protected set; } = [];

        /// <summary>
        /// Retorna o valor atual.
        /// </summary>
        /// <returns></returns>
        public object GetValue() => this.Value;

        /// <summary>
        /// Converte PagedInfo em um PagedResult<typeparamref name="T"/>.
        /// </summary>
        /// <param name="pagedInfo"></param>
        /// <returns></returns>
        public PagedResult<T> ToPagedResult(PagedInfo pagedInfo)
        {
            var pagedResult = new PagedResult<T>(pagedInfo, Value)
            {
                Status = Status,
                SuccessMessage = SuccessMessage,
                CorrelationId = CorrelationId,
                Errors = Errors,
                ValidationErrors = ValidationErrors
            };

            return pagedResult;
        }

        /// <summary>
        /// Representa uma operação bem-sucedida e aceita um valor como resultado da operação.
        /// </summary>
        /// <param name="value">Define a propriedade Value</param>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Success(T value) => new(value);

        /// <summary>
        /// Representa uma operação bem-sucedida e aceita um valor como resultado da operação.
        /// Define a propriedade SuccessMessage para o valor fornecido.
        /// </summary>
        /// <param name="value">Define a propriedade Value</param>
        /// <param name="successMessage">Define a propriedade SuccessMessage</param>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Success(T value, string successMessage) => new(value, successMessage);

        /// <summary>
        /// Representa uma operação bem-sucedida que resultou na criação de um novo recurso.
        /// </summary>
        /// <typeparam name="T">O tipo do recurso criado.</typeparam>
        /// <returns>Um Result<typeparamref name="T"/> com o status Criado.</returns>
        public static Result<T> Created(T value) => new(ResultStatus.Created) { Value = value };

        /// <summary>
        /// Representa uma operação bem-sucedida que resultou na criação de um novo recurso.
        /// Define a propriedade SuccessMessage para o valor fornecido.
        /// </summary>
        /// <typeparam name="T">O tipo do recurso criado.</typeparam>
        /// <param name="value">O valor do recurso criado.</param>
        /// <param name="location">A URL que indica onde o novo recurso criado pode ser acessado.</param>
        /// <returns>Um Result<typeparamref name="T"/> com o status Criado.</returns>
        public static Result<T> Created(T value, string location) => new(ResultStatus.Created) { Value = value, Location = location };

        /// <summary>
        /// Representa um erro que ocorreu durante a execução do serviço.
        /// Uma única mensagem de erro pode ser fornecida e será exposta pela propriedade Errors.
        /// </summary>
        /// <param name="errorMessage">Mensagem de erro.</param>
        /// <returns>Retorno do método.</returns>
        public static Result<T> Error(string errorMessage) => new(ResultStatus.Error) { Errors = new[] { errorMessage } };

        /// <summary>
        /// Representa um erro que ocorreu durante a execução do serviço.
        /// Mensagens de erro podem ser fornecidas e serão expostas pela propriedade Errors.
        /// </summary>
        /// <param name="error">Uma instância opcional de ErrorList com uma lista de mensagens de erro em string e CorrelationId.</param>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Error(ErrorList error = null) => new(ResultStatus.Error)
        {
            CorrelationId = error?.CorrelationId ?? string.Empty,
            Errors = error?.ErrorMessages ?? []
        };

        /// <summary>
        /// Representa um erro de validação que impede o serviço subjacente de ser concluído.
        /// </summary>
        /// <param name="validationError">O erro de validação encontrado.</param>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Invalid(ValidationError validationError)
            => new(ResultStatus.Invalid) { ValidationErrors = [validationError] };

        /// <summary>
        /// Representa erros de validação que impedem o serviço subjacente de ser concluído.
        /// </summary>
        /// <param name="validationErrors">Uma lista de erros de validação encontrados.</param>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Invalid(params ValidationError[] validationErrors) =>
            new(ResultStatus.Invalid)
            { ValidationErrors = new List<ValidationError>(validationErrors) };

        /// <summary>
        /// Representa erros de validação que impedem o serviço subjacente de ser concluído.
        /// </summary>
        /// <param name="validationErrors">Uma lista de erros de validação encontrados.</param>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Invalid(IEnumerable<ValidationError> validationErrors)
            => new(ResultStatus.Invalid) { ValidationErrors = validationErrors };

        /// <summary>
        /// Representa a situação em que um serviço não conseguiu encontrar um recurso solicitado.
        /// </summary>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> NotFound() => new(ResultStatus.NotFound);

        /// <summary>
        /// Representa a situação em que um serviço não conseguiu encontrar um recurso solicitado.
        /// Mensagens de erro podem ser fornecidas e serão expostas pela propriedade Errors.
        /// </summary>
        /// <param name="errorMessages">Uma lista de mensagens de erro em string.</param>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> NotFound(params string[] errorMessages) => new(ResultStatus.NotFound) { Errors = errorMessages };

        /// <summary>
        /// Os parâmetros da chamada estavam corretos, mas o usuário não tem permissão para realizar alguma ação.
        /// </summary>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Forbidden() => new(ResultStatus.Forbidden);

        /// <summary>
        /// Os parâmetros da chamada estavam corretos, mas o usuário não tem permissão para realizar alguma ação.
        /// </summary>
        /// <param name="errorMessages">Uma lista de mensagens de erro em string.</param> 
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Forbidden(params string[] errorMessages) => new(ResultStatus.Forbidden) { Errors = errorMessages };

        /// <summary>
        /// Isso é semelhante a Forbidden, mas deve ser usado quando o usuário não se autenticou ou tentou se autenticar, mas falhou.
        /// </summary>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Unauthorized() => new(ResultStatus.Unauthorized);

        /// <summary>
        /// Isso é semelhante a Forbidden, mas deve ser usado quando o usuário não se autenticou ou tentou se autenticar, mas falhou.
        /// </summary>
        /// <param name="errorMessages">Uma lista de mensagens de erro em string.</param>  
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Unauthorized(params string[] errorMessages) => new(ResultStatus.Unauthorized) { Errors = errorMessages };

        /// <summary>
        /// Representa uma situação em que um serviço está em conflito devido ao estado atual de um recurso,
        /// como um conflito de edição entre várias atualizações concorrentes.
        /// </summary>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Conflict() => new(ResultStatus.Conflict);

        /// <summary>
        /// Representa uma situação em que um serviço está em conflito devido ao estado atual de um recurso,
        /// como um conflito de edição entre várias atualizações concorrentes.
        /// Mensagens de erro podem ser fornecidas e serão expostas pela propriedade Errors.
        /// </summary>
        /// <param name="errorMessages">Uma lista de mensagens de erro em string.</param>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> Conflict(params string[] errorMessages) => new(ResultStatus.Conflict) { Errors = errorMessages };

        /// <summary>
        /// Representa um erro crítico que ocorreu durante a execução do serviço.
        /// Tudo fornecido pelo usuário era válido, mas o serviço não conseguiu concluir devido a uma exceção.
        /// </summary>
        /// <param name="errorMessages">Uma lista de mensagens de erro em string.</param>
        /// <returns>Um Result<typeparamref name="T"/></returns>
        public static Result<T> CriticalError(params string[] errorMessages) => new(ResultStatus.CriticalError) { Errors = errorMessages };

        /// <summary>
        /// Representa uma situação em que um serviço está indisponível, como quando o armazenamento de dados subjacente está indisponível.
        /// Os erros podem ser transitórios, então o chamador pode desejar tentar a operação novamente.
        /// </summary>
        /// <param name="errorMessages">Uma lista de mensagens de erro em string.</param>
        /// <returns>Retorno do método.</returns>
        public static Result<T> Unavailable(params string[] errorMessages) => new(ResultStatus.Unavailable) { Errors = errorMessages };

        /// <summary>
        /// Representa uma situação em que o servidor atendeu com sucesso a solicitação, mas não há conteúdo para enviar de volta no corpo da resposta.
        /// </summary>
        /// <typeparam name="T">O parâmetro de tipo que representa os dados esperados da resposta.</typeparam>
        /// <returns>Um objeto Result.</returns>
        public static Result<T> NoContent() => new(ResultStatus.NoContent);
    }
}
