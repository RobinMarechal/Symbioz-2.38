using Symbioz.Protocol.Enums;
using Symbioz.Protocol.Types;

#pragma warning disable 659

namespace Symbioz.World.Models.Entities.Look {
    public class ContextSubEntity {
        public SubEntityBindingPointCategoryEnum Category { get; set; }

        public sbyte BindingPointIndex { get; set; }

        public ContextActorLook SubActorLook { get; set; }

        public ContextSubEntity(SubEntity subentity) {
            this.Category = (SubEntityBindingPointCategoryEnum) subentity.bindingPointCategory;
            this.BindingPointIndex = subentity.bindingPointIndex;
            this.SubActorLook = new ContextActorLook(subentity.subEntityLook);
        }

        public ContextSubEntity(SubEntityBindingPointCategoryEnum category,
                                sbyte bindingPointIndex,
                                ContextActorLook subActorLook) {
            this.Category = category;
            this.BindingPointIndex = bindingPointIndex;
            this.SubActorLook = subActorLook;
        }

        public override bool Equals(object obj) {
            return obj != null
                   && obj is ContextSubEntity subEntity
                   && subEntity.BindingPointIndex == this.BindingPointIndex
                   && subEntity.Category == this.Category
                   && subEntity.SubActorLook == this.SubActorLook;
        }

        public ContextSubEntity Clone() {
            return new ContextSubEntity(this.Category, this.BindingPointIndex, this.SubActorLook.Clone());
        }

        public SubEntity ToSubEntity() {
            return new SubEntity((sbyte) this.Category, this.BindingPointIndex, this.SubActorLook.ToEntityLook());
        }
    }
}