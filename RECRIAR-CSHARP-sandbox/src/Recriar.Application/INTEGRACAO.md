# Integracao pendente - Domain e Application

Este pacote altera apenas Domain e Application. WinForms, Infrastructure, SQL e Interfaces foram preservados. A compilacao completa fica pendente da integracao.

## Pedro
- Trocar IChildService por ChildService nas telas e na inicializacao.
- Trocar propriedades de entidades e DTOs por getCampo() e setCampo(valor). Inicializadores de objetos devem virar chamadas aos setters. Para ComboBox/DataGridView que usam nomes de propriedades, preencher os componentes explicitamente ou adaptar a vinculacao.
- Chamadas a utilitarios antes estaticos passam a usar objetos: new ErrorHandler().GetMessage(ex), new ChildValidator().getMaxFamilyMembers(), etc.
- Remover interfaces obsoletas quando todas as referencias forem adaptadas.

## Leandro
- ChildService recebe ChildRepository, RelationshipTypeRepository, DbActivityLogger, FileErrorLogger e HtmlChildFileExporter concretos.
- Export(Child, List<RelationshipType>) substitui Export(Child, IReadOnlyDictionary<int,string>). Buscar o parentesco percorrendo a lista e comparando getId(); obter o nome com getName(). Preservar o comportamento para ID nao encontrado.
- Adaptar todos os acessos a entidades para getters/setters, inclusive IDs gerados pelo banco.
- Remover implementacao das interfaces antigas nos repositorios, loggers e exportador.
- Infrastructure hoje referencia Application. Para evitar dependencia circular, trocar essa referencia por Domain (e eliminar dependencias restantes de Application); depois adicionar referencia de Application para Infrastructure. Se algum utilitario de Application for usado na Infrastructure, adaptar ou mover a logica necessaria antes da troca. Nao fizemos essa troca neste pacote porque exige editar a parte do Leandro.
- RelationshipType.OtherId passa a new RelationshipType().getOtherId().

## Validacao
- Testar cadastro, edicao (inclusive familiares removidos/novos), exclusao, pesquisa, exportacao, logs, datas validas e invalidas e mensagens de erro.
- Nao houve compilacao nem teste com MySQL neste ambiente. As interfaces antigas permanecem apenas para a integracao posterior; nao sao usadas pelo ChildService novo.
