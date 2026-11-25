using Recriar.Application.Dtos;
using Recriar.Application.Exceptions;
using Recriar.Domain.Entities;

namespace Recriar.Application.Validators
{
    public class ChildValidator
    {

        private int maxFamilyMembers = 10;

        public int getMaxFamilyMembers()
        {
            return maxFamilyMembers;
        }

        public List<string> Validate(ChildDto child, List<int> relationshipTypeIds)
        {

            List<string> errors = new List<string>();

            CheckLetters(errors, child.getName(), new FieldNames().getName(), new FieldLimits().getPersonName());
            CheckAge(errors, child.getAge(), new FieldNames().getAge());
            CheckDigits(errors, child.getCpf(), new FieldNames().getCpf(), new FieldLimits().getDocument());
            CheckDigits(errors, child.getRg(), new FieldNames().getRg(), new FieldLimits().getDocument());
            CheckOptional(errors, child.getAddress(), new FieldNames().getAddress(), new FieldLimits().getAddress());
            CheckLetters(errors, child.getNeighborhood(), new FieldNames().getNeighborhood(), new FieldLimits().getNeighborhood());
            CheckLetters(errors, child.getCity(), new FieldNames().getCity(), new FieldLimits().getCity());
            CheckOptional(errors, child.getSchool(), new FieldNames().getSchool(), new FieldLimits().getSchool());
            CheckLetters(errors, child.getMotherName(), new FieldNames().getMotherName(), new FieldLimits().getPersonName());
            CheckLetters(errors, child.getFatherName(), new FieldNames().getFatherName(), new FieldLimits().getPersonName());
            CheckDigits(errors, child.getPhone(), new FieldNames().getPhone(), new FieldLimits().getPhone());
            CheckDate(errors, child.getRegistrationDate());

            CheckLetters(errors, child.getGuardianName(), new FieldNames().getGuardianName(), new FieldLimits().getPersonName());
            CheckDigits(errors, child.getGuardianRg(), new FieldNames().getGuardianRg(), new FieldLimits().getDocument());
            CheckDigits(errors, child.getGuardianCpf(), new FieldNames().getGuardianCpf(), new FieldLimits().getDocument());

            CheckLetters(errors, child.getEmergencyContactName(), new FieldNames().getEmergencyContactName(), new FieldLimits().getPersonName());
            CheckDigits(errors, child.getEmergencyContactPhone(), new FieldNames().getEmergencyContactPhone(), new FieldLimits().getPhone());

            CheckOptional(errors, child.getHealthCenter(), new FieldNames().getHealthCenter(), new FieldLimits().getHealthCenter());
            CheckOptional(errors, child.getFileNumber(), new FieldNames().getFileNumber(), new FieldLimits().getFileNumber());
            CheckOptional(errors, child.getAllergy(), new FieldNames().getAllergy(), new FieldLimits().getAllergy());

            List<FamilyMemberDto> members = child.getFamilyMembers();

            if (members == null) members = new List<FamilyMemberDto>();
            if (members.Count > maxFamilyMembers)
            {
                errors.Add(new ErrorMessages().TooManyFamilyMembers());
            }

            for (int i = 0; i < members.Count; i++)
            {
                CheckFamilyMember(errors, members[i], i + 1, relationshipTypeIds);
            }

            return errors;
        }

        private void CheckFamilyMember(
            List<string> errors, FamilyMemberDto member, int number, List<int> relationshipTypeIds)
        {
            if (!relationshipTypeIds.Contains(member.getRelationshipTypeId()))
            {
                errors.Add(new ErrorMessages().InvalidRelationshipType(new FieldNames().FamilyMemberRelationship(number)));
            }
            else if (member.getRelationshipTypeId() == new RelationshipType().getOtherId())
            {

                string field = new FieldNames().FamilyMemberOtherRelationship(number);

                if (member.getOtherRelationship().Length == 0)
                {
                    errors.Add(new ErrorMessages().OtherRelationshipRequired(field));
                }
                else
                {
                    CheckLetters(errors, member.getOtherRelationship(), field, new FieldLimits().getRelationship());
                }
            }

            CheckLetters(errors, member.getName(), new FieldNames().FamilyMemberName(number), new FieldLimits().getPersonName());
            CheckAge(errors, member.getAge(), new FieldNames().FamilyMemberAge(number));
        }

        private void CheckLetters(List<string> errors, string value, string field, int maxLength)
        {
            if (value.Length == 0)
            {
                errors.Add(new ErrorMessages().FieldRequired(field));
            }
            else if (value.Length > maxLength)
            {
                errors.Add(new ErrorMessages().FieldTooLong(field, maxLength));
            }
            else if (!new TextValidator().HasOnlyLetters(value))
            {
                errors.Add(new ErrorMessages().OnlyLetters(field));
            }
        }

        private void CheckDigits(List<string> errors, string value, string field, int maxLength)
        {
            if (value.Length == 0)
            {
                errors.Add(new ErrorMessages().FieldRequired(field));
            }
            else if (value.Length > maxLength)
            {
                errors.Add(new ErrorMessages().FieldTooLong(field, maxLength));
            }
            else if (!new TextValidator().HasOnlyDigits(value))
            {
                errors.Add(new ErrorMessages().OnlyDigits(field));
            }
        }

        private void CheckOptional(List<string> errors, string value, string field, int maxLength)
        {
            if (value.Length > maxLength)
            {
                errors.Add(new ErrorMessages().FieldTooLong(field, maxLength));
            }
        }

        private void CheckAge(List<string> errors, string value, string field)
        {
            if (value.Length == 0)
            {
                errors.Add(new ErrorMessages().FieldRequired(field));
            }
            else
            {
                try { new AgeValidator().ParseAge(value); }
                catch (FormatException) { errors.Add(new ErrorMessages().InvalidAge(field)); }
                catch (OverflowException) { errors.Add(new ErrorMessages().InvalidAge(field)); }
                catch (ArgumentException) { errors.Add(new ErrorMessages().InvalidAge(field)); }
            }
        }

        private void CheckDate(List<string> errors, string value)
        {
            if (value.Length == 0)
            {
                errors.Add(new ErrorMessages().FieldRequired(new FieldNames().getRegistrationDate()));
            }
            else
            {
                try { new DateValidator().ParseDate(value); }
                catch (FormatException) { errors.Add(new ErrorMessages().getInvalidRegistrationDate()); }
            }
        }
    }
}
