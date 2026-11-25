# O que foi mantido e por quê

A adaptação da parte de BD usa os comandos apresentados nas aulas enviadas pelo grupo. O C# faz a comunicação com MySQL por `MySqlConnection`, `MySqlCommand`, parâmetros, `ExecuteReader`, `ExecuteNonQuery` e `ExecuteScalar`, seguindo o exemplo de integração enviado anteriormente. Os slides de BD não ensinam todo o C# necessário para construir uma tela; essa comunicação não deve ser apresentada como conteúdo explicado nesses PDFs.

## Relação com o material

| Recurso do banco | Material enviado |
|---|---|
| Entidades, atributos e relacionamentos | Aula 04 - Modelagem de Dados |
| Transformação dos relacionamentos em tabelas e FKs | Aula 06 - Transformações Entre Modelos |
| Organização até a 3FN | Aula 07 - Normalização |
| CREATE DATABASE, CREATE TABLE, PK, FK e NOT NULL | Aula 09 - DDL, páginas 6 a 9 |
| CASCADE e RESTRICT | Aula 09 - DDL, páginas 10 e 11 |
| AUTO_INCREMENT | Aula 09 - DDL, página 12 |
| Índices e índices UNIQUE, inclusive compostos | Aula 09 - DDL, páginas 19 a 23 |
| INSERT, UPDATE e DELETE | Aula 10 - DML |
| LAST_INSERT_ID | Aula 10 - DML, página 20 |
| SELECT, WHERE, LIKE e ORDER BY | Aula 11 - DQL |
| INNER JOIN, subconsulta e apelidos das tabelas | Aula 12 - Junções e Subconsultas |

Os comandos não precisam aparecer todos em cada consulta. Cada um foi usado onde faz sentido. A data/hora do histórico é enviada como valor pelo C#, e os dados de exemplo usam datas escritas no SQL.

## O que saiu da parte de BD

- Views e o segundo schema de leitura.
- Papéis de acesso e os scripts para conceder esses papéis.
- Transações, níveis de isolamento e SELECT FOR UPDATE.
- CHECKs e configurações de caracteres repetidas em cada coluna.
- LOCATE na pesquisa; a consulta agora usa LIKE.
- Interfaces e dependências circulares da integração antiga.
- Script de PowerShell e configuração por duas variáveis de ambiente.
- Classes auxiliares de oficinas que não eram usadas pelo programa.

Foi mantido um arquivo local de conexão, pois o programa precisa saber onde o MySQL está e qual acesso usar. Ele não cria usuários nem altera configurações do servidor.

## Normalização

A análise considera as regras representadas pelo cadastro. Ter um campo Id sozinho não prova que uma tabela esteja na 3FN.

Na 1FN, cada campo guarda um valor e os familiares ficam em linhas separadas, em vez de colunas Familiar1, Familiar2 e assim por diante.

Na 2FN, os atributos descritivos dependem da chave inteira. Na inscrição, a combinação criança/encontro também é uma chave candidata, protegida por UNIQUE. A data da inscrição pertence àquela inscrição; nome da criança e nome da oficina ficam em suas próprias tabelas.

Na 3FN, evitamos guardar um atributo que depende de outro atributo que não seja chave. FamilyMembers guarda RelationshipTypeId, e RelationshipTypes guarda o nome correspondente. O nome do parentesco não é repetido em cada familiar. Activities guarda ProgramId, e o nome e a descrição do programa ficam em Programs. ActivitySessions aponta para a atividade; não repete o nome do programa.

As demais tabelas guardam os atributos do registro identificado pela própria chave. ActivityLogs guarda o texto de um acontecimento passado; não é uma cópia atualizável da ficha. Por isso o histórico é mantido mesmo quando a criança é excluída.

Nesta modelagem, Guardians e FamilyMembers representam pessoas informadas em uma ficha específica, não um cadastro único de todas as pessoas da ONG. Não há uma tabela de pessoa compartilhada entre várias fichas. Essa é a regra usada para interpretar os relacionamentos apresentados.

## Correspondência com os requisitos de avaliação

| Exigência | Entrega |
|---|---|
| MER com entidades e atributos principais | diagramas/MER_Chen_detalhado.pdf e SVG |
| DER completo | diagramas/DER_11_tabelas.pdf e SVG |
| EER correspondente ao banco implementado | diagramas/EER_11_tabelas.pdf e SVG; origem documentada |
| Script completo a partir de base vazia | database.sql |
| DDL, DML e DQL organizados | 01_estrutura.sql, 02_dados.sql e 03_consultas_interfaces.sql |
| Consultas separadas e identificadas por finalidade | pasta consultas, arquivos 01 a 12 |
| PK, FK, índices e autoincrementos | 01_estrutura.sql |
| Demonstração da normalização | explicação acima e tabelas dos diagramas |
| Aplicação efetivamente usando o banco | repositórios concretos chamados por ChildService |
| Evidência de funcionamento | TESTES.txt e projeto de teste de integração |

O script usa AUTO_INCREMENT nas dez tabelas com IDs gerados. RelationshipTypes tem os nove IDs fixos usados pelo campo parentesco do programa. O Id 9 precisa continuar sendo Outro.

As restrições do banco não substituem todas as validações da aplicação. Sem transações, não existe garantia de salvar várias tabelas como uma operação indivisível. Essa limitação foi mantida explícita para não atribuir ao projeto uma proteção que ele não tem.
