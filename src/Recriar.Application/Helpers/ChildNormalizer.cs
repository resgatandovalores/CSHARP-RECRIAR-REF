using Recriar.Application.Dtos;

namespace Recriar.Application.Helpers
{
    public class ChildNormalizer
    {

        public ChildDto Normalize(ChildDto source)
        {
            ChildDto result = new ChildDto();
            result.setId(source.getId());
            result.setName(Capitalize(source.getName()));
            result.setAge(Clean(source.getAge()));
            result.setCpf(Clean(source.getCpf()));
            result.setRg(Clean(source.getRg()));
            result.setAddress(Clean(source.getAddress()));
            result.setNeighborhood(Capitalize(source.getNeighborhood()));
            result.setCity(Capitalize(source.getCity()));
            result.setSchool(Capitalize(source.getSchool()));
            result.setMotherName(Capitalize(source.getMotherName()));
            result.setFatherName(Capitalize(source.getFatherName()));
            result.setPhone(Clean(source.getPhone()));
            result.setRegistrationDate(Clean(source.getRegistrationDate()));
            result.setGuardianName(Capitalize(source.getGuardianName()));
            result.setGuardianRg(Clean(source.getGuardianRg()));
            result.setGuardianCpf(Clean(source.getGuardianCpf()));
            result.setEmergencyContactName(Capitalize(source.getEmergencyContactName()));
            result.setEmergencyContactPhone(Clean(source.getEmergencyContactPhone()));
            result.setHealthCenter(Clean(source.getHealthCenter()));
            result.setFileNumber(Clean(source.getFileNumber()));
            result.setAllergy(Clean(source.getAllergy()));
            if (source.getFamilyMembers() != null)
            {
                foreach (FamilyMemberDto member in source.getFamilyMembers())
                {
                    result.getFamilyMembers().Add(NormalizeMember(member));
                }
            }
            return result;
        }

        private FamilyMemberDto NormalizeMember(FamilyMemberDto source)
        {
            FamilyMemberDto result = new FamilyMemberDto();
            result.setId(source.getId());
            result.setRelationshipTypeId(source.getRelationshipTypeId());
            result.setOtherRelationship(Capitalize(source.getOtherRelationship()));
            result.setName(Capitalize(source.getName()));
            result.setAge(Clean(source.getAge()));
            return result;
        }

        private string Clean(string text)
        {
            if (text == null) return "";
            return text.Trim();
        }

        private string Capitalize(string text)
        {
            return new TextFormatter().CapitalizeWords(Clean(text));
        }
    }
}
