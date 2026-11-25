# RECRIAR

Abra `Recriar.sln` no Visual Studio. Defina `Recriar.WinForms` como projeto de inicialização.

## Organização

- `Banco`: os cinco arquivos da entrega de banco de dados.
- `src`: código do programa.
- `tests`: teste de integração com MySQL.

## Arquivos do banco

| Arquivo | Conteúdo |
|---|---|
| Banco.sql | Script completo: estrutura, dados fictícios e consultas |
| Consultas.sql | Consultas das telas e auxiliares de gravação, separadas do instalador |
| MER.pdf | Entidades principais, atributos e relacionamentos |
| DER.pdf | As 11 tabelas, campos, chaves e relacionamentos |
| EER.pdf | Diagrama obtido da estrutura implementada no MySQL de teste |

O EER foi produzido lendo a estrutura das tabelas no servidor com SHOW CREATE TABLE. Não é uma captura do Workbench nem um arquivo .mwb. A estrutura conferida corresponde à do projeto instalado no MySQL principal.

## Instalação e conexão

O banco desta máquina já está instalado como `recriar_prod`, na porta 3306. Não execute `Banco.sql` novamente sobre ele. As 11 tabelas anteriores estão preservadas em `recriar_backup_20261125`. Os dois cadastros anteriores foram mantidos e foi acrescentado um cadastro fictício. O banco antigo de leitura não é usado pelo programa atual.

Em uma máquina sem esse banco, abra `Banco/Banco.sql` no Workbench e execute o arquivo inteiro. Ele cria as 11 tabelas, índices, parentescos e dados de demonstração. Use um acesso local que possa criar o banco. O script não cria usuários ou senhas.

No projeto `Recriar.WinForms`, copie `conexao.exemplo.txt` e renomeie a cópia para `conexao.txt`. Preencha os dados do seu MySQL em uma única linha:

```text
Server=localhost;Port=3306;Database=recriar_prod;User ID=SEU_USUARIO;Password=SUA_SENHA;
```

Esse usuário precisa consultar e alterar `recriar_prod`. Nas propriedades do arquivo no Visual Studio, deixe **Copiar para Diretório de Saída** como **Copiar se for mais recente**. O projeto já inclui essa configuração. Compile e pressione F5. Para executar diretamente, o arquivo deve ficar ao lado de `Recriar.WinForms.exe`.

O arquivo `conexao.txt` contém seu acesso local e está no `.gitignore`. Não o envie ao GitHub. Não é necessário PowerShell ou variável de ambiente.

## Onde abrir o código de BD

Os arquivos estão diretamente em `src/Recriar.Infrastructure`:

- `ChildRepository.cs`: cadastro, consulta, edição e exclusão.
- `RelationshipTypeRepository.cs`: lista de parentescos.
- `DbActivityLogger.cs`: histórico das ações.
- `AppDbContext.cs` e `AppDbContextFactory.cs`: conexão com MySQL e leitura da configuração local.
- `FileErrorLogger.cs`: registro de erros em arquivo.
- `HtmlChildFileExporter.cs`: exportação da ficha.

O serviço que valida o cadastro fica em `src/Recriar.Application/Services/ChildService.cs`. As telas estão em `src/Recriar.WinForms/Forms`.

## Consultas

Em `Consultas.sql`, cada consulta está separada por uma linha em branco e aparece nesta ordem:

1. Listar crianças.
2. Pesquisar por parte do nome.
3. Buscar a criança da ficha.
4. Buscar seu responsável.
5. Buscar seu contato de emergência.
6. Buscar familiares com o nome do parentesco por INNER JOIN.
7. Listar as opções de parentesco.
8. Conferir a criança antes de editar.
9. Buscar o responsável antes de salvar.
10. Buscar o contato antes de salvar.
11. Buscar os IDs dos familiares antes de editar.
12. Recuperar o ID logo após um INSERT com LAST_INSERT_ID().

Os SETs antes das consultas preparam variáveis para os exemplos no Workbench. No C#, os valores são enviados como parâmetros do comando. LAST_INSERT_ID() deve ser consultado logo após a inserção, na mesma conexão; executá-lo isoladamente não cadastra nada. A pesquisa usa os padrões `%` e `_` do LIKE.

## Regras e normalização

O banco contém Children, Guardians, EmergencyContacts, FamilyMembers, RelationshipTypes, ActivityLogs, Programs, Activities, ActivitySessions, SessionEnrollments e AttendanceRecords. As telas atuais trabalham com o cadastro da criança. As tabelas de oficinas possuem dados de demonstração, mas não receberam novas telas.

As PKs identificam os registros. As FKs protegem os vínculos. Os índices UNIQUE impedem dois responsáveis ou contatos na mesma ficha, repetição da inscrição criança/encontro e duas presenças para a mesma inscrição. O parentesco Outro mantém o Id 9.

Na 1FN, os familiares ficam em linhas próprias, sem colunas Familiar1, Familiar2 etc. Na 2FN, os atributos dependem da chave inteira: a data da inscrição pertence à inscrição da criança naquele encontro. Na 3FN, o nome do parentesco fica em RelationshipTypes, o nome do programa em Programs e o da atividade em Activities, evitando repetir esses dados nas tabelas que guardam suas FKs. Ter apenas um Id não comprova normalização.

Responsável e familiar representam pessoas informadas em uma ficha específica, não um cadastro único de pessoas compartilhadas entre todas as fichas. ActivityLogs guarda o registro de uma ação passada e continua existindo após excluir a criança.

O C# valida idade, nomes, documentos, telefone, data, responsável, contato e limite de dez familiares. A FK no responsável não obriga toda criança a ter responsável; o serviço confere essa obrigatoriedade. Uma alteração direta no banco pode desrespeitar regras verificadas somente no C#.

Esta versão usa um único banco, sem views, CHECKs, papéis de acesso ou transações. Uma falha durante uma gravação em várias tabelas pode deixar uma parte gravada. Não há garantia de rollback.

## Conteúdo das aulas e verificação

Modelagem e transformação dos relacionamentos seguem as aulas 04 e 06; normalização, a aula 07. PK, FK, AUTO_INCREMENT, índices e regras de exclusão aparecem na aula 09. INSERT, UPDATE, DELETE e LAST_INSERT_ID() estão na aula 10. SELECT, LIKE e ORDER BY estão na aula 11; junções e subconsultas, na aula 12.

O código de comunicação C#/MySQL segue o exemplo de integração enviado pelo grupo. Os slides de BD não ensinam todo o C# necessário para uma aplicação Windows Forms.

A solução compilou com zero erros e zero avisos. Foram aprovadas 32 verificações de integração em uma base separada, incluindo cadastro, edição, familiares, exclusão, pesquisa, exportação, validações, FK, UNIQUE e CASCADE. Também foram conferidas a listagem, a abertura da ficha e a seleção de parentesco pelas telas. Isso não inclui teste de carga ou garantia de edição concorrente.

No MySQL principal foram confirmadas 11 tabelas, 11 PKs, 9 FKs e 12 índices nomeados, com preservação dos cadastros anteriores. A reorganização das pastas não altera os dados do MySQL.
