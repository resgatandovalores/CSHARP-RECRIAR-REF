using Recriar.Application.Dtos;
using Recriar.Application.Exceptions;
using Recriar.Application.Helpers;
using Recriar.Infrastructure.Persistence.Repositories;
using Recriar.Infrastructure.Logging;
using Recriar.Infrastructure.Export;
using Recriar.Application.Mappers;
using Recriar.Application.Validators;
using Recriar.Domain.Entities;

namespace Recriar.Application.Services
{
    public class ChildService
    {

        private ChildRepository _children;
        private RelationshipTypeRepository _relationshipTypes;
        private DbActivityLogger _activityLogger;
        private FileErrorLogger _errorLogger;
        private HtmlChildFileExporter _fileExporter;

        public ChildService(
            ChildRepository children,
            RelationshipTypeRepository relationshipTypes,
            DbActivityLogger activityLogger,
            FileErrorLogger errorLogger,
            HtmlChildFileExporter fileExporter)
        {
            _children = children;
            _relationshipTypes = relationshipTypes;
            _activityLogger = activityLogger;
            _errorLogger = errorLogger;
            _fileExporter = fileExporter;
        }

        public int Register(ChildDto child)
        {
            try
            {
                ChildDto data = PrepareAndValidate(child);

                Child entity = new ChildMapper().ToNewEntity(data);
                _children.Add(entity);

                LogActivity("Cadastro criado", entity);
                return entity.getId();
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                _errorLogger.LogError(ex, "Cadastro de criança");
                throw new ArgumentException(new ErrorMessages().getUnexpected());
            }
        }

        public void Update(int id, ChildDto child)
        {
            try
            {

                Child? entity = _children.GetById(id);
                if (entity == null) throw new ArgumentException(new ErrorMessages().getChildNotFound());

                ChildDto data = PrepareAndValidate(child);

                new ChildMapper().ApplyTo(entity, data);
                _children.Update(entity);

                LogActivity("Cadastro editado", entity);
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                _errorLogger.LogError(ex, "Edição de criança");
                throw new ArgumentException(new ErrorMessages().getUnexpected());
            }
        }

        public void Delete(int id)
        {
            try
            {

                Child? entity = _children.GetById(id);
                if (entity == null) throw new ArgumentException(new ErrorMessages().getChildNotFound());

                if (!_children.Delete(id))
                {
                    throw new ArgumentException(new ErrorMessages().getChildNotFound());
                }

                LogActivity("Cadastro excluído", entity);
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                _errorLogger.LogError(ex, "Exclusão de criança");
                throw new ArgumentException(new ErrorMessages().getUnexpected());
            }
        }

        public List<ChildSummaryDto> GetAll()
        {
            try { return Summaries(_children.GetAll()); }
            catch (Exception ex) { _errorLogger.LogError(ex, "Listagem de crianças"); throw new ArgumentException(new ErrorMessages().getUnexpected()); }
        }

        public List<ChildSummaryDto> SearchByName(string name)
        {
            try
            {
                string text = "";
                if (name != null) text = name.Trim();

                List<Child> found;
                if (text.Length == 0) found = _children.GetAll();
                else found = _children.SearchByName(text);

                return Summaries(found);
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                _errorLogger.LogError(ex, "Pesquisa por nome");
                throw new ArgumentException(new ErrorMessages().getUnexpected());
            }
        }

        public ChildDto GetById(int id)
        {
            try
            {
                Child? entity = _children.GetById(id);
                if (entity == null) throw new ArgumentException(new ErrorMessages().getChildNotFound());
                return new ChildMapper().ToDto(entity);
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                _errorLogger.LogError(ex, "Consulta de criança");
                throw new ArgumentException(new ErrorMessages().getUnexpected());
            }
        }

        public string ExportFile(int id)
        {
            try
            {
                Child? entity = _children.GetById(id);
                if (entity == null) throw new ArgumentException(new ErrorMessages().getChildNotFound());

                List<RelationshipType> relationshipNames = new List<RelationshipType>();
                if (entity.getFamilyMembers().Count > 0)
                {
                    foreach (RelationshipType item in _relationshipTypes.GetAll()) relationshipNames.Add(item);
                }

                string path;
                try
                {
                    path = _fileExporter.Export(entity, relationshipNames);
                }
                catch (IOException ex)
                {

                    _errorLogger.LogError(ex, "Exportação de ficha");
                    throw new ArgumentException(new ErrorMessages().getExportFailed());
                }
                catch (UnauthorizedAccessException ex)
                {
                    _errorLogger.LogError(ex, "Exportação de ficha");
                    throw new ArgumentException(new ErrorMessages().getExportFailed());
                }

                LogActivity("Ficha exportada", entity);
                return path;
            }
            catch (ArgumentException) { throw; }
            catch (Exception ex)
            {
                _errorLogger.LogError(ex, "Exportação de ficha");
                throw new ArgumentException(new ErrorMessages().getUnexpected());
            }
        }

        public List<RelationshipType> GetRelationshipTypes()
        {
            try { return _relationshipTypes.GetAll(); }
            catch (Exception ex) { _errorLogger.LogError(ex, "Consulta de parentescos"); throw new ArgumentException(new ErrorMessages().getUnexpected()); }
        }

        // Confere todos os campos antes de salvar.
        private ChildDto PrepareAndValidate(ChildDto child)
        {

            if (child == null) throw new ArgumentException("Cadastro vazio.");

            ChildDto data = new ChildNormalizer().Normalize(child);

            List<int> relationshipIds = new List<int>();
            if (data.getFamilyMembers().Count > 0)
            {
                foreach (RelationshipType item in _relationshipTypes.GetAll()) relationshipIds.Add(item.getId());
            }

            List<string> errors = new ChildValidator().Validate(data, relationshipIds);

            if (errors.Count > 0)
            {
                throw new ArgumentException(string.Join(Environment.NewLine, errors));
            }

            return data;
        }

        private void LogActivity(string action, Child child)
        {
            try
            {
                _activityLogger.Log(action, $"Criança {child.getId()} - {child.getName()}");
            }
            catch (Exception ex)
            {
                _errorLogger.LogError(ex, $"Log de atividades ({action})");
            }
        }

        private List<ChildSummaryDto> Summaries(List<Child> children)
        {
            List<ChildSummaryDto> result = new List<ChildSummaryDto>();
            foreach (Child child in children) result.Add(new ChildMapper().ToSummary(child));
            return result;
        }
    }
}
