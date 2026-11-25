using Recriar.Application.Dtos;
using Recriar.Application.Validators;
using Recriar.Domain.Entities;

namespace Recriar.Application.Mappers
{
    public class ChildMapper
    {

        private string dateFormat = "dd/MM/yyyy";

        public Child ToNewEntity(ChildDto dto)
        {
            Child child = new Child();
            ApplyTo(child, dto);
            return child;
        }

        public void ApplyTo(Child child, ChildDto dto)
        {
            child.setName(dto.getName());
            child.setAge(ToAge(dto.getAge()));
            child.setCpf(dto.getCpf());
            child.setRg(dto.getRg());
            child.setAddress(dto.getAddress());
            child.setNeighborhood(dto.getNeighborhood());
            child.setCity(dto.getCity());
            child.setSchool(dto.getSchool());
            child.setMotherName(dto.getMotherName());
            child.setFatherName(dto.getFatherName());
            child.setPhone(dto.getPhone());
            child.setRegistrationDate(ToDate(dto.getRegistrationDate()));
            child.setHealthCenter(dto.getHealthCenter());
            child.setFileNumber(dto.getFileNumber());
            child.setAllergy(dto.getAllergy());

            Guardian? guardian = child.getGuardian();
            if (guardian == null) guardian = new Guardian();
            guardian.setName(dto.getGuardianName());
            guardian.setRg(dto.getGuardianRg());
            guardian.setCpf(dto.getGuardianCpf());
            child.setGuardian(guardian);

            EmergencyContact? contact = child.getEmergencyContact();
            if (contact == null) contact = new EmergencyContact();
            contact.setName(dto.getEmergencyContactName());
            contact.setPhone(dto.getEmergencyContactPhone());
            child.setEmergencyContact(contact);

            ApplyFamilyMembers(child, dto.getFamilyMembers());
        }

        private void ApplyFamilyMembers(Child child, List<FamilyMemberDto> members)
        {

            // Guarda os IDs dos familiares que continuam no cadastro.
            List<int> keptIds = new List<int>();

            foreach (FamilyMemberDto memberDto in members)
            {

                FamilyMember? member = null;
                if (memberDto.getId() > 0)
                {
                    foreach (FamilyMember existing in child.getFamilyMembers())
                    {
                        if (existing.getId() == memberDto.getId()) { member = existing; break; }
                    }
                }

                if (member == null)
                {

                    member = new FamilyMember();
                    child.getFamilyMembers().Add(member);
                }
                else
                {
                    keptIds.Add(member.getId());
                }

                member.setRelationshipTypeId(memberDto.getRelationshipTypeId());

                member.setOtherRelationship(null);
                if (memberDto.getRelationshipTypeId() == new RelationshipType().getOtherId()) member.setOtherRelationship(memberDto.getOtherRelationship());

                member.setName(memberDto.getName());
                member.setAge(ToAge(memberDto.getAge()));
            }

            // Percorre de tras para frente para remover sem pular registros.
            for (int i = child.getFamilyMembers().Count - 1; i >= 0; i--)
            {
                if (child.getFamilyMembers()[i].getId() > 0 && !keptIds.Contains(child.getFamilyMembers()[i].getId())) child.getFamilyMembers().RemoveAt(i);
            }
        }

        public ChildDto ToDto(Child child)
        {
            ChildDto result = new ChildDto();
            result.setId(child.getId());
            result.setName(child.getName());
            result.setAge(child.getAge().ToString());
            result.setCpf(child.getCpf());
            result.setRg(child.getRg());
            result.setAddress(child.getAddress());
            result.setNeighborhood(child.getNeighborhood());
            result.setCity(child.getCity());
            result.setSchool(child.getSchool());
            result.setMotherName(child.getMotherName());
            result.setFatherName(child.getFatherName());
            result.setPhone(child.getPhone());
            result.setRegistrationDate(child.getRegistrationDate().ToString(dateFormat));
            result.setHealthCenter(child.getHealthCenter());
            result.setFileNumber(child.getFileNumber());
            result.setAllergy(child.getAllergy());
            Guardian? guardian = child.getGuardian();
            if (guardian != null)
            {
                result.setGuardianName(guardian.getName());
                result.setGuardianRg(guardian.getRg());
                result.setGuardianCpf(guardian.getCpf());
            }
            EmergencyContact? contact = child.getEmergencyContact();
            if (contact != null)
            {
                result.setEmergencyContactName(contact.getName());
                result.setEmergencyContactPhone(contact.getPhone());
            }
            foreach (FamilyMember member in child.getFamilyMembers()) result.getFamilyMembers().Add(ToDto(member));
            return result;
        }

        public FamilyMemberDto ToDto(FamilyMember member)
        {
            FamilyMemberDto result = new FamilyMemberDto();
            result.setId(member.getId());
            result.setRelationshipTypeId(member.getRelationshipTypeId());
            result.setOtherRelationship(CleanRelationship(member.getOtherRelationship()));
            result.setName(member.getName());
            result.setAge(member.getAge().ToString());
            return result;
        }

        public ChildSummaryDto ToSummary(Child child)
        {
            ChildSummaryDto result = new ChildSummaryDto();
            result.setId(child.getId());
            result.setName(child.getName());
            result.setCpf(child.getCpf());
            result.setPhone(child.getPhone());
            return result;
        }

        private string CleanRelationship(string? text)
        {
            if (text == null) return "";
            return text;
        }
        private int ToAge(string text) { return new AgeValidator().ParseAge(text); }
        private DateTime ToDate(string text) { return new DateValidator().ParseDate(text); }
    }
}
