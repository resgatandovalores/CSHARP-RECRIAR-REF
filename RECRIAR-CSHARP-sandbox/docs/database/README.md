# Banco de dados do RECRIAR

Esta versão usa um banco MySQL, chamado `recriar_prod`, com 11 tabelas. A leitura e a gravação acontecem nesse mesmo banco. Não há views, réplica, papéis de acesso, procedures, triggers ou transações no código entregue.

## Instalação

1. Abra o MySQL Workbench e conecte ao seu MySQL local.
2. Abra `database.sql` e execute o arquivo inteiro. Ele cria as tabelas, os índices, os parentescos, os dados fictícios e executa as consultas de demonstração.
3. Confira se o banco `recriar_prod` apareceu na lista de schemas.

O instalador é para uma base vazia. Se `recriar_prod` já existir, não execute por cima dele. Ele não apaga nem converte bancos antigos. Nesta entrega, o banco anterior da máquina foi preservado; os testes foram feitos em outro servidor local.

Também é possível executar `01_estrutura.sql`, `02_dados.sql` e `03_consultas_interfaces.sql`, nessa ordem. Escolha essa opção ou o arquivo completo, para não repetir a instalação.

## Conexão do programa

Não precisa usar PowerShell nem configurar variáveis de ambiente.

No Visual Studio, abra `Recriar.sln` e defina `Recriar.WinForms` como projeto de inicialização. Dentro desse projeto, copie `conexao.exemplo.txt`, renomeie a cópia para `conexao.txt` e preencha com o usuário e a senha do seu MySQL. Mantenha o arquivo em uma única linha:

```text
Server=localhost;Port=3306;Database=recriar_prod;User ID=SEU_USUARIO;Password=SUA_SENHA;
```

Esse usuário precisa conseguir consultar e alterar `recriar_prod`. O script não cria usuários nem troca senhas. Use um acesso local já configurado no seu MySQL.

Nas propriedades de `conexao.txt` no Visual Studio, deixe **Copiar para Diretório de Saída** como **Copiar se for mais recente**. O projeto já inclui essa configuração para o arquivo. Depois compile e pressione F5. Para abrir o executável diretamente, o arquivo precisa ficar ao lado de `Recriar.WinForms.exe`.

`conexao.txt` fica somente na máquina de cada integrante e está no `.gitignore`. Não coloque sua senha no arquivo de exemplo, no código ou no GitHub. O arquivo local guarda a senha em texto; não o compartilhe.

## As 11 tabelas

| Tabela | Para que serve |
|---|---|
| Children | Dados da criança |
| Guardians | Responsável registrado na ficha |
| EmergencyContacts | Contato de emergência da ficha |
| FamilyMembers | Familiares informados na ficha |
| RelationshipTypes | Lista de parentescos; Outro tem Id 9 |
| ActivityLogs | Histórico das ações no cadastro |
| Programs | Programas da ONG |
| Activities | Oficinas de cada programa |
| ActivitySessions | Encontros de uma oficina |
| SessionEnrollments | Inscrições das crianças nos encontros |
| AttendanceRecords | Presença de cada inscrição |

As telas atuais trabalham com o cadastro da criança, responsável, contato e familiares. As cinco tabelas de oficinas têm dados de demonstração, mas não receberam telas nesta adaptação.

## Consultas das telas

A pasta `consultas` identifica cada consulta pelo nome do arquivo:

| Arquivo | Finalidade |
|---|---|
| 01 | Listagem da tela principal |
| 02 | Pesquisa por parte do nome, usando LIKE |
| 03 a 06 | Abertura da ficha: criança, responsável, contato e familiares |
| 07 | Opções do campo parentesco |
| 08 a 11 | Consultas auxiliares usadas para editar o cadastro |
| 12 | Recuperação do ID imediatamente depois de um INSERT |

A exportação reaproveita a leitura da ficha. Nos arquivos SQL, `@id`, `@childId` e `@name` são variáveis para executar os exemplos no Workbench. No C#, esses nomes são parâmetros de `MySqlCommand`. O exemplo usa o primeiro cadastro disponível; no programa, o ID vem da ficha escolhida pelo usuário.

`LAST_INSERT_ID()` só tem o significado esperado logo após a inserção, na mesma conexão. A consulta isolada do arquivo 12 não faz um cadastro. Na pesquisa, `%` e `_` seguem o comportamento de padrões do LIKE ensinado na aula.

## Onde está a integração

- `ChildRepository.cs`: SELECT, INSERT, UPDATE e DELETE do cadastro, com parâmetros.
- `RelationshipTypeRepository.cs`: consulta os parentescos.
- `DbActivityLogger.cs`: insere uma linha no histórico.
- `AppDbContext.cs`: abre conexões com o único banco.
- `AppDbContextFactory.cs`: lê o arquivo local de conexão.
- `ChildService.cs`: recebe o cadastro, valida os campos e chama o repositório.

Foram seguidas as assinaturas novas do grupo: classes concretas, getters e setters, e exportação com `List<RelationshipType>`. A referência entre os projetos ficou WinForms → Application → Infrastructure → Domain, sem dependência circular. As telas receberam os ajustes necessários para chamar essas classes.

## Regras e limites desta versão

O banco garante PK, FK, NOT NULL, UNIQUE e as regras de exclusão. Responsável e contato têm ChildId único. A inscrição tem UNIQUE(SessionId, ChildId), e a presença tem inscrição única.

O C# valida idade, nome, documentos, telefone, data, responsável, contato e limite de dez familiares. Não há CHECK de idade no MySQL. Uma alteração feita diretamente no banco pode passar pelas chaves e mesmo assim desrespeitar uma regra validada somente pelo programa.

Uma FK no responsável não obriga toda criança a ter um responsável. Essa obrigatoriedade é conferida pelo serviço antes do cadastro.

Não há COMMIT, ROLLBACK ou transação envolvendo as várias gravações. Cada comando é executado separadamente: uma falha durante o salvamento pode deixar uma parte do cadastro gravada. A validação anterior ajuda a evitar entradas inválidas, mas não oferece a garantia de uma transação. Não apresentem essa versão como se ela tivesse gravação atômica.

## Modelagem e testes

Os arquivos MER, DER e EER estão em `diagramas`. O MER destaca as entidades principais. O DER detalha as 11 tabelas. O EER foi desenhado a partir da estrutura efetivamente lida do MySQL de teste, conforme `EER_ORIGEM.txt`.

A justificativa da normalização e a relação com as aulas estão em `CONTEUDO_E_REQUISITOS.md`. O resultado da validação está em `TESTES.txt`.
